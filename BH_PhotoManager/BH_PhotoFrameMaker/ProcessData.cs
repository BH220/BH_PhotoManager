using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH_PhotoFrameMaker
{
    public class ProcessData
    {
        public bool IsSuccess { get; set; } = false;
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }
        public ProcessData(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            ErrorMessage = "";
        }

        public ProcessData()
        {
        }
    }
}
