using IISLP.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISLP.CLI
{
    // quick and dirty cli
    public class Program
    {
        public static void Main(string[] args)
        {
            string type = args[0];
            string path = args[1];

            ILogParser parser = null;
            switch (type.Trim().ToLower())
            {
                case "w3c":
                    parser = new W3CParser();
                    break;
                case "iis":
                    break;
                case "ncsa":
                    break;
            }

            var items = parser.ParseLog(path);

            Console.WriteLine($"Total items: {items.Count()}");

            var distinctIps = items.Select(c => c.ClientIP).Distinct();
            Console.WriteLine($"Distinct items: {distinctIps.Count()}");
            foreach (var item in distinctIps)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
