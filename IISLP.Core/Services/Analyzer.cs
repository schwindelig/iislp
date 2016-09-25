using IISLP.Core.Models;
using IISLP.Core.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IISLP.Core.Services
{
    public class Analyzer
    {
        public Task<ClientData[]> AnalyzeLog(Stream stream, LogFormat format)
        {
            // bug .net core: https://github.com/dotnet/corefx/issues/10024
            var bug10024 = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            BaseParser parser = null;
            parser = GetParserForFormat(format);

            var logEntries = parser.ParseLog(stream);
            var result = Task.WhenAll(logEntries.GroupBy(l => l.ClientIP).Select(async g => new ClientData
            {
                IP = g.Key,
                RequestCount = g.Count(),
                HostEntry = await this.GetHostEntry(g.Key)
            }));

            return result;
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
                return await Dns.GetHostEntryAsync(ip);
            }
            catch { return null; }
        }
    }
}