using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace BH_PhotoFrameMakerConsole
{
    public class RemakePhotoManager
    {
        private readonly HashSet<string> imageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp"
        };

        private readonly string baseDir = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);

        private string pathOrigin => Path.Combine(baseDir, "origin");
        private string pathRemake => Path.Combine(baseDir, "remake");
        private string pathFail => Path.Combine(baseDir, "fail");
        private string logPath => Path.Combine(pathRemake, "log.txt");

        private readonly int width = 1920;
        private readonly int height = 1080;

        // HDD 가정: 과도한 병렬 I/O는 역효과 → 보수적으로 (CPU/2, 최대 4)
        private readonly int maxDegreeOfParallelism =
            Math.Max(1, Math.Min(4, Math.Max(1, Environment.ProcessorCount / 2)));

        // ✅ B안: 탐색 후보를 "6개"로 축소 (미세탐색 제거)
        // - 품질 70% 미만 금지
        // - 불필요한 반복 인코딩 감소(성능↑)
        private static readonly int[] CandidateQualities = new[] { 95, 90, 85, 80, 75, 70};

        private static RemakePhotoManager? _instance;
        public static RemakePhotoManager Instance => _instance ??= new RemakePhotoManager();

        private readonly object _logLock = new();

        public async Task Run()
        {
            Directory.CreateDirectory(pathOrigin);
            Directory.CreateDirectory(pathRemake);
            Directory.CreateDirectory(pathFail);

            Console.WriteLine($"병렬 처리 크기(Parallel Size): {maxDegreeOfParallelism}");

            // 1) fail 우선 처리
            var failFiles = GetImageFiles(pathFail);
            if (failFiles.Length > 0)
            {
                Console.WriteLine($"실패 파일 수(Fail file count): {failFiles.Length}");
                Console.WriteLine("실패 폴더 선처리 시작(Starting pre-processing of the fail folder)");
                Console.WriteLine("");

                await ProcessBatch(
                    inputFolder: pathFail,
                    files: failFiles,
                    moveFailedToFailFolder: false,
                    moveSuccessBackToOrigin: true
                );
            }

            // 2) origin 처리
            var originFiles = GetImageFiles(pathOrigin);
            Console.WriteLine($"원본 파일 수(Origin File Count): {originFiles.Length}");
            Console.WriteLine("원본 파일 처리 시작(Starting origin processing)");
            Console.WriteLine("");

            await ProcessBatch(
                inputFolder: pathOrigin,
                files: originFiles,
                moveFailedToFailFolder: true,
                moveSuccessBackToOrigin: false
            );
        }

        private string[] GetImageFiles(string folder)
        {
            if (!Directory.Exists(folder))
                return Array.Empty<string>();

            // 비재귀(하위폴더 없음)
            return Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => imageExtensions.Contains(Path.GetExtension(f)))
                .ToArray();
        }

        private async Task ProcessBatch(
            string inputFolder,
            string[] files,
            bool moveFailedToFailFolder,
            bool moveSuccessBackToOrigin)
        {
            int total = files.Length;
            int done = 0;
            int failed = 0;
            int skipped = 0;

            await Parallel.ForEachAsync(
                files,
                new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism },
                async (file, ct) =>
                {
                    int now = Interlocked.Increment(ref done);

                    try
                    {
                        string outputPath = GetOutputJpegPath(file);

                        // 이미 처리된 파일 스킵
                        if (File.Exists(outputPath))
                        {
                            Interlocked.Increment(ref skipped);
                            if (now % 500 == 0)
                                Console.WriteLine($"Process: {now}/{total} (Skip:{skipped}, Fail:{failed})");
                            return;
                        }

                        if (now % 500 == 1)
                            Console.WriteLine($"Process: {now}/{total} (Skip:{skipped}, Fail:{failed})");

                        await Task.Run(() => ProcessOneToJpeg(file, outputPath), ct);

                        if (moveSuccessBackToOrigin)
                        {
                            MoveBackToOrigin(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Interlocked.Increment(ref failed);
                        LogFail(file, ex);

                        if (moveFailedToFailFolder)
                        {
                            try
                            {
                                MoveToFail(file);
                            }
                            catch (Exception moveEx)
                            {
                                Console.WriteLine($"실패 파일 이동도 실패(Failed to move the failed file) => {moveEx.Message}");
                                LogFail(file, new Exception($"실패 파일 이동도 실패: {moveEx.Message}", moveEx));
                            }
                        }
                    }
                });

            Console.WriteLine($"Input : {inputFolder}");
            Console.WriteLine($"Total : {total}, Skip: {skipped}, Fail: {failed}, Try count: {total - skipped}");
        }

        private void ProcessOneToJpeg(string inputPath, string outputPath)
        {
            using Image image = Image.Load(inputPath);

            // 품질 영향 없는 용량 감소: 메타데이터 제거
            image.Metadata.ExifProfile = null;
            image.Metadata.IccProfile = null;
            image.Metadata.XmpProfile = null;

            // 크든 작든 1920x1080 박스 안으로(비율 유지), 작으면 확대, 크면 축소, 패딩/크롭 없음
            ResizeToFitWithUpscaleTuning(image);

            // ✅ B안: 6개 후보만 탐색하여 "가장 작은 용량" 선택
            SaveJpegBestOfCandidatesToFileStream(image, outputPath);
        }

        private void ResizeToFitWithUpscaleTuning(Image image)
        {
            var target = new Size(width, height);
            bool needUpscale = image.Width < width || image.Height < height;

            if (needUpscale)
            {
                // 단계적 업스케일(2배씩)
                while (image.Width * 2 < width && image.Height * 2 < height)
                {
                    int nextW = image.Width * 2;
                    int nextH = image.Height * 2;

                    image.Mutate(ctx =>
                    {
                        ctx.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(nextW, nextH),
                            Sampler = KnownResamplers.MitchellNetravali
                        });
                    });
                }

                // 최종 박스 맞춤 + 약샤픈
                image.Mutate(ctx =>
                {
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = target,
                        Sampler = KnownResamplers.MitchellNetravali
                    });

                    float sharpen = ComputeSharpenAmount();
                    if (sharpen > 0f)
                        ctx.GaussianSharpen(sharpen);
                });
            }
            else
            {
                // 축소/동일
                image.Mutate(ctx =>
                {
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = target,
                        Sampler = KnownResamplers.Lanczos3
                    });
                });
            }
        }

        private float ComputeSharpenAmount()
        {
            // 보수적 고정값
            return 0.25f;
        }

        private string GetOutputJpegPath(string inputPath)
        {
            string baseName = Path.GetFileNameWithoutExtension(inputPath);
            string ext = Path.GetExtension(inputPath).TrimStart('.');

            string fileName = $"remake_{baseName}_{ext}.jpg";
            return Path.Combine(pathRemake, fileName);
        }

        /// <summary>
        /// ✅ B안: 품질 후보 6개(95/90/85/80/75/70)만 메모리에서 인코딩해 보고,
        ///     가장 작은 바이트를 고른 뒤 디스크(FileStream) 저장은 1회만 수행.
        /// </summary>
        private static void SaveJpegBestOfCandidatesToFileStream(Image image, string outputPath)
        {
            byte[] bestBytes = Array.Empty<byte>();

            foreach (int q in CandidateQualities)
            {
                var bytes = EncodeJpegToBytes(image, q);
                if (bestBytes.Length == 0 || bytes.Length < bestBytes.Length)
                    bestBytes = bytes;
            }

            const int bufferSize = 1024 * 1024; // 1MB
            using var fs = new FileStream(
                outputPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize,
                FileOptions.SequentialScan);

            fs.Write(bestBytes, 0, bestBytes.Length);
            fs.Flush(true);
        }

        private static byte[] EncodeJpegToBytes(Image image, int quality)
        {
            using var ms = new MemoryStream();

            var enc = new JpegEncoder
            {
                Quality = quality,
                ColorType = JpegEncodingColor.YCbCrRatio420,
                Interleaved = true
            };

            image.Save(ms, enc);
            return ms.ToArray();
        }

        private void MoveToFail(string inputPath)
        {
            Directory.CreateDirectory(pathFail);

            string fileName = Path.GetFileName(inputPath);
            string destPath = EnsureUniquePath(Path.Combine(pathFail, fileName));

            File.Move(inputPath, destPath, overwrite: false);
        }

        private void MoveBackToOrigin(string inputPathInFail)
        {
            Directory.CreateDirectory(pathOrigin);

            string fileName = Path.GetFileName(inputPathInFail);
            string destPath = EnsureUniquePath(Path.Combine(pathOrigin, fileName));

            File.Move(inputPathInFail, destPath, overwrite: false);
        }

        private static string EnsureUniquePath(string path)
        {
            if (!File.Exists(path))
                return path;

            string dir = Path.GetDirectoryName(path)!;
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            for (int i = 1; ; i++)
            {
                string candidate = Path.Combine(dir, $"{name}_{i}{ext}");
                if (!File.Exists(candidate))
                    return candidate;
            }
        }

        private void LogFail(string inputPath, Exception ex)
        {
            Directory.CreateDirectory(pathRemake);

            string msg =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FAIL: {inputPath}{Environment.NewLine}" +
                $"{ex}{Environment.NewLine}" +
                $"------------------------------------------------------------{Environment.NewLine}";

            lock (_logLock)
            {
                File.AppendAllText(logPath, msg, new UTF8Encoding(false));
            }
        }
    }
}