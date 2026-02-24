using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BH_PhotoFrameMaker
{
    public class SettingHelper
    {
        public string RootPath { get; set; }
        public int ConvertWidth { get; set; }
        public int ConvertHeight { get; set; }
        public bool IsAutoConvert { get; set; }//true면 자동 변환, false면 수동 변환

        string SettingFilePath
        {
            get 
            {
                string dic = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BH Photo Frame Maker");
                if(Directory.Exists(dic) == false)
                {
                    Directory.CreateDirectory(dic);
                }
                return Path.Combine(dic, "setting.json");
            }
        }
        private static SettingHelper _instance = null;
        public static SettingHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingHelper();
                    _instance.Init();
                }
                return _instance;
            }
        }



        private void Init()
        {
            if (File.Exists(SettingFilePath))
            {
                string json = File.ReadAllText(SettingFilePath);
                SettingData data = JsonConvert.DeserializeObject<SettingData>(json);
                if (data != null)
                {
                    RootPath = data.RootPath;
                    ConvertWidth = data.ConvertWidth;
                    ConvertHeight = data.ConvertHeight;
                    IsAutoConvert = data.IsAutoConvert;
                }
                else
                {
                    DefLoad();
                }
            }
            else
            {
                DefLoad();
            }
        }

        public void Save()
        {
            SettingData data = new SettingData();
            data.RootPath = RootPath;
            data.ConvertWidth = ConvertWidth;
            data.ConvertHeight = ConvertHeight;
            data.IsAutoConvert = IsAutoConvert;
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(SettingFilePath, json);
        }

        private void DefLoad()
        {
            SettingData data = new SettingData();
            RootPath = data.RootPath;
            ConvertWidth = data.ConvertWidth;
            ConvertHeight = data.ConvertHeight;
            IsAutoConvert = data.IsAutoConvert;
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(SettingFilePath, json);
        }
    }

    public class SettingData
    {
        public string RootPath { get; set; } = "";
        public int ConvertWidth { get; set; } = 1920;
        public int ConvertHeight { get; set; } = 1080;
        public bool IsAutoConvert { get; set; } = true;//true면 자동 변환, false면 수동 변환
    }
}
