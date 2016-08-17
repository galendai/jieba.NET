using System.IO;
using Microsoft.Extensions.Configuration;

namespace JiebaNet.Segmenter
{
    public class ConfigManager
    {
        public static string ConfigFileBaseDir
        {
            get
            {
                var config = new ConfigurationBuilder()
                                .AddXmlFile("app.config")
                                .Build();
                return config["JiebaConfigFileDir"] ?? "Resource";
                //return ConfigurationManager.AppSettings["JiebaConfigFileDir"] ?? "Resources";
            }
        }

        public static string MainDictFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "dict.txt"); }
        }

        public static string ProbTransFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "prob_trans.json"); }
        }

        public static string ProbEmitFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "prob_emit.json"); }
        }

        public static string PosProbStartFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_start.json"); }
        }

        public static string PosProbTransFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_trans.json"); }
        }

        public static string PosProbEmitFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_emit.json"); }
        }

        public static string CharStateTabFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "char_state_tab.json"); }
        }
    }
}