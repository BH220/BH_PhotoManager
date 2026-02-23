using System.Runtime.InteropServices;
using System.Text;

namespace BH_PhotoFrameMakerConsole
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCP(uint wCodePageID);

        static void Main(string[] args)
        {
            SetConsoleOutputCP(65001);
            SetConsoleCP(65001);

            Console.OutputEncoding = new UTF8Encoding(false);
            Console.InputEncoding = new UTF8Encoding(false);

            CheckFolder();

            try
            {
                Task.Run(() => RemakePhotoManager.Instance.Run());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CheckFolder()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string pathOrigin = $"{currentDir}\\origin";
            string pathRemake = $"{currentDir}\\remake";
            if(Directory.Exists(pathOrigin) == false)
            {
                Directory.CreateDirectory(pathOrigin);
            }
            if (Directory.Exists(pathRemake) == false)
            {
                Directory.CreateDirectory(pathRemake);
            }

            string[] files = Directory.GetFiles(pathOrigin);

            if(files.Length == 0)
            {
                string pathFail = $"{currentDir}\\fail";
                if(Directory.Exists(pathFail))
                {
                    string[] failFiles = Directory.GetFiles(pathFail);
                    if(failFiles.Length != 0)
                    {
                        //처리할 실패 사진이 있음
                        return;
                    }
                }
                Console.WriteLine("아래 실행경로에 위치한 origin 폴더에 변환전 원본 사진을 넣고 다시 실행해 주세요");
                Console.WriteLine("[Please put the original photos in the \"origin\" folder below and run it again.]");
                Console.WriteLine("");
                Console.Write(">> ");
                Console.WriteLine(pathOrigin);

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("이 창을 닫으려면 아무키나 누르세요..");
                Console.WriteLine("[Press any key to close this window..]");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
