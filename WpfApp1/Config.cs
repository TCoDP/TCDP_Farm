using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Config
    {
        public class Settings
        {
            [JsonProperty("steam_path")]
            public string SteamPath { get; set; }

            [JsonProperty("cs_path")]
            public string CsPath { get; set; }

            [JsonProperty("mode")]
            public string Mode { get; set; }

            [JsonProperty("maFiles_path")]
            public string MaFilesPath { get; set; }

            [JsonProperty("sounds")]
            public bool Sounds { get; set; }

            [JsonProperty("reconnect_delay")]
            public int ReconnectDelay { get; set; }
        }

        public const string configFile = "config.json";

        [JsonProperty("custom")]
        public Settings Custom { get; set; }
        //public Dictionary<string, string> Custom { get; set; }

        [JsonProperty("clear_date")]
        public string ClearDate { get; set; }
        private static Config _settings { get; set; }


        public static Config GetConfig()
        {
            string jsonSettings = File.ReadAllText(configFile);
            _settings = JsonConvert.DeserializeObject<Config>(jsonSettings);

            if (_settings == null)
            {
                _settings = Config.GenerateNewConfig();
            }

            return _settings;
        }

        public void Save(Config x)
        {
            string newConfig = JsonConvert.SerializeObject(x);
            File.WriteAllText(configFile, newConfig);
        }

        private static Config GenerateNewConfig()
        {
            Config x = new Config();
            x.Custom = DefaultSettings();
            x.ClearDate = "1/1/2021";

            return x;
        }

        private static Settings DefaultSettings()
        {
            const string _steam_path = "C:/Program Files (x86)/Steam/",
                _cs_path = "steamapps/common/Counter-Strike Global Offensive/",
                _mode = "15-15",
                _maFiles_path = _cs_path;
            const bool _sounds = true;
            const int _reconnect_delay = 1;

            Settings x = new Settings();

            x.SteamPath = _steam_path;
            x.CsPath = _cs_path;
            x.Mode = _mode;
            x.MaFilesPath = _maFiles_path;
            x.Sounds = _sounds;
            x.ReconnectDelay = _reconnect_delay;

            return x;
        }
    }
}
