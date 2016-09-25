using IISLP.Core.Models;
using IISLP.Core.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IISLP.Core.Services
{
    public class Analyzer
    {
        public Task<ClientData[]> AnalyzeLog(string path, LogFormat format)
        {
            // bug .net core: https://github.com/dotnet/corefx/issues/10024
            var bug10024 = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            BaseParser parser = null;
            parser = GetParserForFormat(format);

            if (File.Exists(path))
            {
                var logEntries = parser.ParseLog(path);
                var result = Task.WhenAll(logEntries.GroupBy(l => l.ClientIP).Select(async g => new ClientData
                {
                    IP = g.Key,
                    RequestCount = g.Count(),
                    HostEntry = await this.GetHostEntry(g.Key)
                }));

                return result;
            }
            else throw new Exception($"File {path} cannot be found.");
        }

        private static BaseParser GetParserForFormat(LogFormat format)
        {
            BaseParser parser;
            switch (format)
            {
                case LogFormat.W3C:
                    parser = new W3CParser();
                    break;
                case LogFormat.IIS:
                    parser = new IISParser();
                    break;
                case LogFormat.NCSA:
                    parser = new NCSAParser();
                    break;
                default:
                    throw new Exception($"Unknown log format: {format}");
            }

            return parser;
        }

        protected async Task<IPHostEntry> GetHostEntry(IPAddress ip)
        {
            try
            {
                Console.WriteLine("-> Resolving " + ip);
                var res = await Dns.GetHostEntryAsync(ip);
                Console.WriteLine("--> Resolved " + ip + " to " + res.HostName);
                return res;
            }
            catch { return null; }
        }

        protected async Task ResolveIP(ClientData clientData)
        {
            Console.WriteLine("Resolving " + clientData.IP);
            try
            {
                clientData.HostEntry = await Dns.GetHostEntryAsync(clientData.IP);
                Console.WriteLine("Resolved " + clientData.HostEntry.HostName);
            }
            catch (Exception e) { Console.WriteLine("Exception: " + e); }
        }
    }
}
