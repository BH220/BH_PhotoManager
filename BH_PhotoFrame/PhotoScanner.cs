using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH_PhotoFrame
{
    public class PhotoScanner
    {
        public static List<string> Scan(string folder)
        {
            if (!Directory.Exists(folder))
                return new List<string>();

            var files = Directory.GetFiles(folder).Where(f =>f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)).ToList();

            Random rng = new Random();
            return files.OrderBy(x => rng.Next()).ToList();
        }
    }
}
