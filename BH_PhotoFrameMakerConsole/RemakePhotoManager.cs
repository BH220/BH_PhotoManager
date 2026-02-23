using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
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

        private static RemakePhotoManager _instance;
        public static RemakePhotoManager Instance => _instance ??= new RemakePhotoManager();

        private readonly object _logLock = new();

        /// <summary>
        /// 실행 순서:
        /// 1) fail 폴더가 있으면 fail 먼저 처리 (성공 시 origin으로 되돌림)
        /// 2) origin 처리 (실패 시 fail로 이동)
        /// </summary>
        public async Task Run()
        {
            Directory.CreateDirectory(pathOrigin);
            Directory.CreateDirectory(pathRemake);
            Directory.CreateDirectory(pathFail);

            // 1) fail 우선 처리
            var failFiles = GetImageFiles(pathFail);
            if (failFiles.Length > 0)
            {
                Console.WriteLine($"fail 폴더 선처리 시작: {failFiles.Length}개");
                await ProcessBatch(
                    inputFolder: pathFail,
                    files: failFiles,
                    moveFailedToFailFolder: false,
                    moveSuccessBackToOrigin: true
                );
            }

            // 2) origin 처리
            var originFiles = GetImageFiles(pathOrigin);
            if (originFiles.Length == 0)
            {
                Console.WriteLine("origin 폴더에 처리할 이미지가 없습니다.");
                return;
            }

            Console.WriteLine($"origin 처리 시작: {originFiles.Length}개");
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
                        // 이미 처리된 파일 스킵 (리런 중복 방지)
                        string outputPath = GetOutputJpegPath(file);
                        if (File.Exists(outputPath))
                        {
                            Interlocked.Increment(ref skipped);
                            if (now % 500 == 0)
                                Console.WriteLine($"진행: {now}/{total} (skip:{skipped}, fail:{failed})");
                            return;
                        }

                        if (now % 500 == 1)
                            Console.WriteLine($"진행: {now}/{total} (skip:{skipped}, fail:{failed})");

                        await Task.Run(() => ProcessOneToJpeg(file, outputPath), ct);

                        if (moveSuccessBackToOrigin)
                        {
                            // fail에서 성공하면 원본을 origin으로 되돌림
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
                                LogFail(file, new Exception($"실패 파일 이동도 실패: {moveEx.Message}", moveEx));
                            }
                        }
                    }
                });

            Console.WriteLine($"입력폴더: {inputFolder}");
            Console.WriteLine($"전체: {total}, 스킵: {skipped}, 실패: {failed}, 처리시도: {total - skipped}");
            Console.WriteLine($"병렬도(MaxDegreeOfParallelism): {maxDegreeOfParallelism}");
        }

        private void ProcessOneToJpeg(string inputPath, string outputPath)
        {
            using Image image = Image.Load(inputPath);

            // 품질 영향 없는 용량 감소: 메타데이터 제거
            image.Metadata.ExifProfile = null;
            image.Metadata.IccProfile = null;
            image.Metadata.XmpProfile = null;

            // 1920x1080 초과 시 비율 유지 축소
            if (image.Width > width || image.Height > height)
            {
                image.Mutate(ctx =>
                {
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(width, height),
                        Sampler = KnownResamplers.Lanczos3
                    });
                });
            }

            // JPEG 최적 저장 (디스크 저장은 FileStream으로 1회)
            SaveJpegOptimizedToFileStream(image, outputPath);
        }

        private string GetOutputJpegPath(string inputPath)
        {
            // fail<->origin 이동해도 출력명이 동일해야 스킵이 안정적
            // 경로 해시 금지, 파일명 + 원본 확장자 기반으로 고정
            string baseName = Path.GetFileNameWithoutExtension(inputPath);
            string ext = Path.GetExtension(inputPath).TrimStart('.');

            // 예: remake_photo_png.jpg
            string fileName = $"remake_{baseName}_{ext}.jpg";
            return Path.Combine(pathRemake, fileName);
        }

        private static void SaveJpegOptimizedToFileStream(Image image, string outputPath)
        {
            const int minQuality = 70;
            const int maxQuality = 95;

            byte[] bestBytes = Array.Empty<byte>();
            int bestQ = maxQuality;

            // 1차: 5단위 탐색(메모리에서만)
            for (int q = maxQuality; q >= minQuality; q -= 5)
            {
                var bytes = EncodeJpegToBytes(image, q);
                if (bestBytes.Length == 0 || bytes.Length < bestBytes.Length)
                {
                    bestBytes = bytes;
                    bestQ = q;
                }
            }

            // 2차: bestQ 주변 미세탐색(±4)
            int start = Math.Max(minQuality, bestQ - 4);
            int end = Math.Min(maxQuality, bestQ + 4);
            for (int q = end; q >= start; q--)
            {
                var bytes = EncodeJpegToBytes(image, q);
                if (bytes.Length < bestBytes.Length)
                {
                    bestBytes = bytes;
                    bestQ = q;
                }
            }

            // 디스크 저장 1회: FileStream (HDD에 유리하게 큰 버퍼 + SequentialScan)
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
                Quality = quality,                         // 70~95 범위에서 사용
                ColorType = JpegEncodingColor.YCbCrRatio420, // 4:2:0 (용량↓, 품질 체감 유지)
                Interleaved = true                          // 기본 권장(일반적인 JPEG)
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
                File.AppendAllText(logPath, msg, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }
    }
}