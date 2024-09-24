using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH_PhotoManager
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CheckDirectory();
            Application.Run(new frmMain());
        }

        private static void CheckDirectory()
        {
            string root_path = Application.StartupPath;
            string root_media = root_path + @"\media";
            string root_archive = root_media + @"\archive";
            string root_frame = root_media + @"\frame";
            string root_print = root_media + @"\print";
            if (Directory.Exists(root_media) == false)
            {
                if (Directory.Exists(root_archive) == false) Directory.CreateDirectory(root_archive);
                if (Directory.Exists(root_frame) == false) Directory.CreateDirectory(root_frame);
                if (Directory.Exists(root_print) == false) Directory.CreateDirectory(root_print);
            }
            else
            {
                if (Directory.Exists(root_archive) == false) Directory.CreateDirectory(root_archive);
                if (Directory.Exists(root_frame) == false) Directory.CreateDirectory(root_frame);
                if (Directory.Exists(root_print) == false) Directory.CreateDirectory(root_print);
            }
        }
    }
}
