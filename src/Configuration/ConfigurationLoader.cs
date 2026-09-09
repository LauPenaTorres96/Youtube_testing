using Newtonsoft.Json;
using System;
using System.IO;

namespace YoutubeTests.Configuration
{
    public class ConfigurationLoader
    {
        public static AppSettings LoadSettings(string filePath = "appsettings.json")
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file not found: {filePath}");
            }

            string json = File.ReadAllText(filePath);
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(json);

            if (settings == null)
            {
                throw new InvalidOperationException("Failed to deserialize app settings.");
            }

            return settings;
        }

        public static T LoadTestData<T>(string filePath = "config/testdata.json")
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Test data file not found: {filePath}");
            }

            string json = File.ReadAllText(filePath);
            T data = JsonConvert.DeserializeObject<T>(json);

            if (data == null)
            {
                throw new InvalidOperationException("Failed to deserialize test data.");
            }

            return data;
        }
    }
}
