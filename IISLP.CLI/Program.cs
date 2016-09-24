using IISLP.Core.Parsers;
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
            var stopwatch = Stopwatch.StartNew();

            string format = args[0];
            string path = args[1];

            Console.WriteLine($"Parsing {path}");
            Console.WriteLine($"Log format: {format}\r\n");

            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            ILogParser parser = null;
            switch (format.ToLower())
            {
                case "w3c":
                    parser = new W3CParser();
                    break;
                case "iis":
                    parser = new IISParser();
                    break;
                case "ncsa":
                    parser = new NCSAParser();
                    break;
                default:
                    Console.WriteLine($"Uknown format ${format}");
                    return;
            }

            var items = parser.ParseLog(path).ToList();
            var measure = stopwatch.ElapsedMilliseconds;

            var distinctIps = items.Select(c => c.ClientIP).Distinct();

            Console.WriteLine($"Total items: {items.Count()}");
            Console.WriteLine($"Distinct items: {distinctIps.Count()}");
            foreach (var item in distinctIps)
            {
                Console.WriteLine(item);
            }
            
            Console.WriteLine($"\r\nFile processed in {measure}ms");
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
