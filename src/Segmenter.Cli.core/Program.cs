using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.CommandLineUtils;
//using CommandLine;
using JiebaNet.Segmenter.PosSeg;

namespace JiebaNet.Segmenter.Cli
{
    class Options
    {
        //[Option('h', "help")]
        //public bool ShowHelp { get; set; }

        //[Option('v', "version")]
        //public bool ShowVersion { get; set; }

        //[Option('f', "file", Required = true)]
        public string FileName { get; set; }

        //[Option('d', "delimiter", DefaultValue = "/ ")]
        public string Delimiter { get; set; }

        //[Option('p', "pos")]
        public bool POS { get; set; }

        ////[Option('u', "user-dict")]
        ////public string UserDict { get; set; }

        //[Option('a', "cut-all")]
        public bool CutAll { get; set; }

        //[Option('n', "no-hmm")]
        public bool NoHmm { get; set; }
        
        public bool Verbose { get; set; }

        ////[Option('q', "quiet")]
        ////public bool Quiet { get; set; }

        //[ParserState]
        //public IParserState LastParserState { get; set; }

        //[HelpOption]
        //public string GetUsage()
        //{
        //    var usage = new StringBuilder();
        //    usage.AppendLine();
        //    usage.AppendLine("jieba.NET options and parameters: ");
        //    usage.AppendLine();
        //    usage.AppendLine("-f \t --file \t the file name, required.");
        //    usage.AppendLine();
        //    usage.AppendLine("-d \t --delimiter \t the delimiter between tokens, default: / .");
        //    usage.AppendLine("-a \t --cut-all \t use cut_all mode.");
        //    usage.AppendLine("-n \t --no-hmm \t don't use HMM.");
        //    usage.AppendLine("-p \t --pos \t enable POS tagging.");


        //    usage.AppendLine("-v \t --version \t show version info.");
        //    usage.AppendLine("-h \t --help \t show help details.");
        //    usage.AppendLine();
        //    usage.AppendLine("sample usages: ");
        //    usage.AppendLine("$ jiebanet -f input.txt > output.txt");
        //    usage.AppendLine("$ jiebanet -d | -f input.txt > output.txt");
        //    usage.AppendLine("$ jiebanet -p -f input.txt > output.txt");

        //    return usage.ToString();
        //}
    }

    public class Program
    {
        public static void Main(string[] args)
        {
//#if DEBUG
//            var options = new Options
//            {
//                FileName = @"c:\test.txt",
//                NoHmm = false,
//                CutAll = true 
//            };
//            SegmentFile(options);
//            Console.ReadLine();
//#else
            // set the basic application info
            var mainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            var app = new CommandLineApplication();
            app.Name = System.IO.Path.GetFileName(mainModule.FileName);
            app.FullName = $"{System.Environment.NewLine}jieba.NET DotNet Core Cli Tool";

            // set the option arguments
            var optionVerbose = app.Option("-v | --verbose", "Show verbose output", CommandOptionType.NoValue);
            var optionPos = app.Option("-p | --pos", "POS Value, enable POS tagging.", CommandOptionType.NoValue);
            var optionCutAll = app.Option("-a | --cut-all", "use cut all mode", CommandOptionType.NoValue);
            var optionNoHmm = app.Option("-n | --no-hmm", "don't use HMM.", CommandOptionType.NoValue);
            var optionDelimiter = app.Option("-d | --delimiter", "the delimiter between tokens, defult: / .", CommandOptionType.SingleValue);

            // set the input parameters
            var fileName = app.Argument("[filename]", "The text file name needs to be processed, this is the required field");

            // Excute the command 
            app.OnExecute(() =>
            {
                var options = new Options();
                // Parse the commandline arguments
                // get the file name
                if (String.IsNullOrEmpty(fileName.Value))
                {
                    AnsiConsole.GetError(true).WriteLine("You must specified a file name to be processed");
                    return -1;
                }
                if (!File.Exists(fileName.Value))
                {
                    AnsiConsole.GetError(true).WriteLine("The process file does not exists");
                    return -2;
                }
                options.FileName = Path.GetFullPath(fileName.Value);

                // parse the options
                if (optionVerbose.HasValue())
                {
                    options.Verbose = true;
                }
                if (optionPos.HasValue())
                {
                    options.POS = true;
                }
                if (optionCutAll.HasValue())
                {
                    options.CutAll = true;
                }
                if (optionNoHmm.HasValue())
                {
                    options.NoHmm = true;
                }
                if (optionDelimiter.HasValue())
                {
                    options.Delimiter = optionDelimiter.Value();
                }

                // Execute the command
                SegmentFile(options);
                return 0;
            });

            app.HelpOption("-h | --help | -?");
            app.Execute(args);
//#endif
        }


        private static void SegmentFile(Options options)
        {
            var result = new List<string>();

            var fileName = Path.GetFullPath(options.FileName);
            //var fileName = options.FileName;
            var lines = File.ReadAllLines(fileName);

            Func<string, bool, bool, IEnumerable<string>> cutMethod = null;
            var segmenter = new JiebaSegmenter();
            if (options.POS)
            {
                cutMethod = (text, cutAll, hmm) =>
                {
                    var posSeg = new PosSegmenter(segmenter);
                    return posSeg.Cut(text, hmm).Select(token => string.Format("{0}/{1}", token.Word, token.Flag));
                };
            }
            else
            {
                cutMethod = segmenter.Cut;
            }

            var delimiter = string.IsNullOrWhiteSpace(options.Delimiter) ? "/ " : options.Delimiter;
            foreach (var line in lines)
            {
                try
                {
                    result.Add(string.Join(delimiter, cutMethod(line, options.CutAll, options.NoHmm)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            Console.WriteLine(string.Join(Environment.NewLine, result));
        }
    }
}
