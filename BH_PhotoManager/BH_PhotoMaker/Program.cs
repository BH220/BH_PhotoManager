using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System.Globalization;

namespace BH_PhotoMaker
{
    internal class Program
    {
        const int TargetW = 1920;
        const int TargetH = 1080;

        static readonly string[] ImageExts = { ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff", ".webp" };


        static void Main(string[] args)
        {
            Console.WriteLine("============ BH_PhotoMaker 시작 ============");
            Console.WriteLine("1: 이미지 압축");
            Console.WriteLine("2: 이미지 재정렬");
            Console.Write("실행할 작업의 번호를 입력 후 엔터: ");
            string key = Console.ReadLine();
            while(string.IsNullOrWhiteSpace(key) || (key != "1" && key != "2"))
            {
                Console.Write("잘못된 입력입니다. 다시 입력해주세요 (1 또는 2): ");
                key = Console.ReadLine();
            }
            if (key == "1")
            {
                ImageCompres();
            }
            else if(key == "2")
            {
                ImageSort();
            }
        }

        #region 이미지 재정렬
        static void ImageSort()
        {
            var baseDir = AppContext.BaseDirectory;
            var sortDir = Path.Combine(baseDir, "sort");
            Directory.CreateDirectory(sortDir);
            var files = Directory.EnumerateFiles(sortDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => ImageExts.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("sort 폴더에 변환할 이미지가 없습니다.");
                return;
            }

            var newDir = Path.Combine(baseDir, "sort_new");
            if (Directory.Exists(newDir)) Directory.Delete(newDir, true);
            Directory.CreateDirectory(newDir);

            Console.Write("");
            Console.Write("");
            Console.WriteLine("===  이미지 재정렬  ===");
            int n = files.Count;
            Console.WriteLine($"대상 이미지 수: {n:#,#}");
            var arr = new int[n];
            for (int i = 0; i < n; i++) arr[i] = i;

            var rng = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }

            string oFile = "";
            string nFile = "";
            for(int idx = 1; idx <= n; idx++)
            {
                Console.WriteLine($"{idx}/{n} 처리중..");
                oFile = files[arr[idx - 1]];
                nFile = Path.Combine(newDir, $"{idx:0000000}.jpg");
                File.Move(oFile, nFile, true);
            }
            Directory.Delete(sortDir, true);
            Directory.Move(newDir, sortDir);

            Console.WriteLine("처리 완료");

        }
        #endregion

        #region 이미지 압축
        static int DefaultJpegQuality = 60;
        static void ImageCompres()
        {
            var baseDir = AppContext.BaseDirectory;
            var originDir = Path.Combine(baseDir, "origin");
            var outDir = Path.Combine(baseDir, "convert");

            Directory.CreateDirectory(originDir);
            Directory.CreateDirectory(outDir);

            var files = Directory.EnumerateFiles(originDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => ImageExts.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("origin 폴더에 변환할 이미지가 없습니다.");
                return;
            }

            Console.Write("");
            Console.Write("");
            Console.WriteLine("===  이미지 압축  ===");

            int startNumber;
            while (true)
            {
                Console.Write("시작할 이미지 번호를 입력하세요: ");
                var key = Console.ReadLine();

                if (int.TryParse(key, out startNumber))
                    break;

                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            }
            
           
            Console.WriteLine($"총 {files.Count}개 처리 시작 (JPG 품질={DefaultJpegQuality})");

            int ok = 0, fail = 0;
            foreach (var inputPath in files)
            {
                try
                {
                    ProcessOne(inputPath, outDir, startNumber);
                    ok++;
                }
                catch (Exception ex)
                {
                    fail++;
                    Console.WriteLine($"[실패] {startNumber.ToString("0000000")} from {Path.GetFileName(inputPath)} --> {ex.Message}");
                }
                startNumber++;
            }

            Console.WriteLine($"완료: 성공 {ok}, 실패 {fail}");
            Console.WriteLine($"출력 폴더: {outDir}");
        }
        static void ProcessOne(string inputPath, string outDir, int fileNum)
        {
            using var image = Image.Load<Rgba32>(inputPath);
            image.Mutate(x => x.AutoOrient());

            int srcW = image.Width;
            int srcH = image.Height;

            // 비율 유지 축소 (업스케일 금지)
            double scale = Math.Min((double)TargetW / srcW, (double)TargetH / srcH);
            if (scale > 1.0) scale = 1.0;

            int newW = (int)Math.Floor(srcW * scale);
            int newH = (int)Math.Floor(srcH * scale);

            // 리사이즈
            Image<Rgba32> resized;
            if (newW != srcW || newH != srcH)
            {
                resized = image.Clone(ctx =>
                    ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(newW, newH),
                        Mode = ResizeMode.Stretch,
                        Sampler = KnownResamplers.Lanczos3
                    })
                );
            }
            else
            {
                resized = image.Clone();
            }

            // 1920x1080 검은 배경 캔버스
            using var canvas = new Image<Rgba32>(TargetW, TargetH, Color.Black);

            // 중앙 배치
            int offsetX = (TargetW - resized.Width) / 2;
            int offsetY = (TargetH - resized.Height) / 2;
            canvas.Mutate(ctx => ctx.DrawImage(resized, new Point(offsetX, offsetY), 1f));

            // EXIF 촬영 날짜 읽기
            string? takenDate = null;
            var exif = image.Metadata.ExifProfile;
            if (exif != null)
            {
                var entry = exif.Values.FirstOrDefault(v => v.Tag == ExifTag.DateTimeOriginal);
                if (entry is IExifValue<DateTime> dateVal)
                {
                    // 값이 DateTime으로 들어온 경우
                    var dt = dateVal.Value;
                    takenDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (entry is IExifValue<string> strVal)
                {
                    // 문자열로 들어오는 경우: "yyyy:MM:dd HH:mm:ss"
                    if (DateTime.TryParseExact(
                            strVal.Value,
                            "yyyy:MM:dd HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeLocal,
                            out var dt))
                    {
                        takenDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }

            // 워터마크 추가 (검은 글자 + 흰색 테두리)
            if (!string.IsNullOrEmpty(takenDate))
            {
                var font = SystemFonts.CreateFont("Arial", 36);
                const int margin = 20;

                // 크기 측정
                var textSize = TextMeasurer.MeasureSize(takenDate, new TextOptions(font));
                float x = TargetW - margin - textSize.Width;
                float y = TargetH - margin - textSize.Height;

                canvas.Mutate(ctx =>
                {
                    // 흰색 테두리
                    int outline = 2;
                    for (int dx = -outline; dx <= outline; dx++)
                    {
                        for (int dy = -outline; dy <= outline; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;
                            ctx.DrawText(takenDate, font, Color.Black, new PointF(x + dx, y + dy));
                        }
                    }
                    // 본문(검정)
                    ctx.DrawText(takenDate, font, Color.White, new PointF(x, y));
                });
            }

            // 메타데이터 제거
            canvas.Metadata.ExifProfile = null;
            canvas.Metadata.IccProfile = null;

            var outName = fileNum.ToString("0000000") + ".jpg";
            var outPath = Path.Combine(outDir, outName);

            var encoder = new JpegEncoder { Quality = DefaultJpegQuality };
            if (!File.Exists(outPath))
            {
                canvas.Save(outPath, encoder);
                resized.Dispose();

                Console.WriteLine($"[성공] {outName} from {Path.GetFileName(inputPath)}");
            }
            else
            {
                throw new Exception($"이미 존재하는 파일");
            }
        }
        #endregion
    }
}