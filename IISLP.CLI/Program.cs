using IISLP.Core.Parsers;
using IISLP.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IISLP.CLI
{
    // quick and dirty cli
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        public static async Task MainAsync(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            string formatStr = args[0];
            string path = args[1];

            Console.WriteLine($"Parsing {path}");
            Console.WriteLine($"Log format: {formatStr}\r\n");

            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            LogFormat format;
            switch (formatStr.ToLower())
            {
                case "w3c":
                    format = LogFormat.W3C;
                    break;
                case "iis":
                    format = LogFormat.IIS;
                    break;
                case "ncsa":
                    format = LogFormat.NCSA;
                    break;
                default:
                    Console.WriteLine($"Uknown format ${formatStr}");
                    return;
            }

            var analyzer = new Analyzer();
            var clientInfos = await analyzer.AnalyzeLog(path, format);
        }
    }
}
