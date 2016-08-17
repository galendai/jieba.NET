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
                                .AddXmlFile("app.config")
                                .Build();
                return config["JiebaConfigFileDir"] ?? "Resources";
                //return ConfigurationManager.AppSettings["JiebaConfigFileDir"] ?? "Resources";
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