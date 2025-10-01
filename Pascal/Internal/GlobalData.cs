using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Pascal.Internal
{
    /// <summary>
    /// DevWinUI.GlobalData인데, internal로 되어있어서 Pascal.Internal로 복사해옴.
    /// </summary>

    internal partial class GlobalData
    {
        public static void Init()
        {
            if (File.Exists(SavePath))
            {
                try
                {
                    var json = File.ReadAllText(SavePath);
                    Config = (string.IsNullOrEmpty(json) ? new AppConfig() : JsonSerializer.Deserialize<AppConfig>(json, (System.Text.Json.Serialization.Metadata.JsonTypeInfo<AppConfig>)GlobalDataJsonContext.Default.AppConfig)) ?? new AppConfig();
                }
                catch
                {
                    Config = new AppConfig();
                }
            }
            else
                Config = new AppConfig();
        }

        public static void Save()
        {
            var json = JsonSerializer.Serialize(Config, (System.Text.Json.Serialization.Metadata.JsonTypeInfo<AppConfig>)GlobalDataJsonContext.Default.AppConfig);
            File.WriteAllText(SavePath, json);
        }

        public static AppConfig Config { get; set; }
        public static string SavePath { get; set; }
    }
}
