using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH_PhotoFrame
{
    public class ImageLoader
    {
        public static SKBitmap Load(string path)
        {
            using var stream = File.OpenRead(path);
            return SKBitmap.Decode(stream);
        }
    }
}
