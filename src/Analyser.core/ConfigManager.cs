using System.IO;
using Microsoft.Extensions.Configuration;

namespace JiebaNet.Analyser
{
    public class ConfigManager
    {
        // TODO: duplicate codes.
        public static string ConfigFileBaseDir
        {
            get
            {
                var config = new ConfigurationBuilder()
                                .AddJsonFile("config.json")
                                .Build();
                var resFolder = config["JeibaConfigFileDir"] ?? "Resources";
                var path = Path.Combine($"{Directory.GetCurrentDirectory()}", resFolder);
                return path; 
            }
        }

        public static string IdfFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "idf.txt"); }
        }

        public static string StopWordsFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "stopwords.txt"); }
        }
    }
}