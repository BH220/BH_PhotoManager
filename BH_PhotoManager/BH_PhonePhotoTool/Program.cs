
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace BH_PhonePhotoTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============ BH_PhonePhotoTool 시작 ============");
            Console.WriteLine("1: 사진 모으기(각자 이름으로 된 폴더에서 [정리]폴더로 이동)");
            Console.WriteLine("");
            Console.Write("실행할 작업의 번호를 입력 후 엔터: ");
            string key = Console.ReadLine();
            if (key == "1")
            {
                PhotoIntegration();
            }
        }

        private static void PhotoIntegration()
        {
            string root = AppContext.BaseDirectory;
            string targetRoot = Path.Combine(root, "정리");
            string[] lstPath = new string[]
            {
                "병호\\사진",
                "병호\\동영상",
                "지애\\사진",
                "지애\\동영상",
                "엄마\\사진",
                "엄마\\동영상",
                "아빠\\사진",
                "아빠\\동영상",
                "누나\\사진",
                "누나\\동영상",
            };
            string dicPath = "";
            string dicPathRoot = "";
            string nFile = "";
            foreach (string path in lstPath)
            {
                dicPathRoot = $"{targetRoot}\\{(path.Contains("사진") ? "사진" : "동영상")}\\";
                if (Directory.Exists($"{root}{path}"))
                {
                    string[] files = Directory.GetFiles($"{root}{path}", "*.*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        try
                        {
                            dicPath = "";
                            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(file);

                            // EXIF 촬영 날짜 읽기 
                            var exif = image.Metadata.ExifProfile;
                            if (exif != null)
                            {
                                var entry = exif.Values.FirstOrDefault(v => v.Tag == ExifTag.DateTimeOriginal);
                                if (entry is IExifValue<DateTime> dateVal)
                                {
                                    // 값이 DateTime으로 들어온 경우
                                    var dt = dateVal.Value;
                                    dicPath = $"{dicPathRoot}\\{dt.Year}\\{dt.ToString("yyyyMMdd-yyyyMMdd_")}\\";
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
                                        dicPath = $"{dicPathRoot}\\{dt.Year}\\{dt.ToString("yyyyMMdd-yyyyMMdd_")}\\";
                                    }
                                }
                            }
                            else
                            {//찍은 날짜 확인이 안되는 경우. 파일명의 패턴으로 읽어야한다.
                                DateTime dt = DateTime.Now;

                                dicPath = $"{dicPathRoot}\\{dt.Year}\\{dt.ToString("yyyyMMdd-yyyyMMdd_")}\\";
                            }

                            if (string.IsNullOrEmpty(dicPath))
                            {//exif와 파일명 패턴으로도 확인이 안되는 이미지는 별도로 분리한다.
                                //dicPath = $"{}";
                                string tfn = Path.GetFileName(file).ToLower()
                                    .Replace("_", "")
                                    .Replace("-", "")
                                    .Replace(".", "")
                                    .Replace(" ", "");
                                if (tfn.StartsWith("kakaotalk"))
                                {
                                    tfn = tfn.Substring(9, tfn.Length - 9);
                                }

                                if(tfn.Length>=)

                                dicPath = $"{dicPathRoot}\\known\\";
                            }

                            if (!Directory.Exists(dicPath))
                                Directory.CreateDirectory(dicPath);
                            nFile = $"{dicPath}{Path.GetFileName(file)}";
                            if (File.Exists(nFile))
                            {
                                int idx = 1;
                                while(true)
                                {
                                    nFile = $"{dicPath}{Path.GetFileNameWithoutExtension(file)}_{idx}{Path.GetExtension(file)}";
                                    if (File.Exists(nFile))
                                        idx++;
                                    else
                                        break;
                                }
                            }

                            File.Move(file, nFile);

                            //    string fileName = Path.GetFileName(file);
                            //string destFile = Path.Combine(target, fileName);
                            //File.Move(file, destFile, true);
                            //Console.WriteLine($"파일 이동: {file} -> {destFile}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"파일 이동 실패: {file} - {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"경로가 존재하지 않음: {path}");
                }
            }
        }
    }
}
