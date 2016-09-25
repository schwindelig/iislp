using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IISLP.Core.Models;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace IISLP.Core.Parsers
{
    public abstract class BaseParser
    {
        public virtual IEnumerable<LogEntry> ParseLog(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                int lineCounter = 0;
                while (reader.Peek() >= 0)
                {
                    LogEntry entry = null;
                    try
                    {
                        entry = this.ProcessLine(reader.ReadLine());
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed to parse line {lineCounter}.", e);
                    }

                    if (entry != null)
                    {
                        yield return entry;
                    }
                }
            }
        }

        protected abstract LogEntry ProcessLine(string line);

        protected LogEntry BuildLogEntry(string clientIp)
        {
            IPAddress ip = null;
            if (IPAddress.TryParse(clientIp, out ip))
            {
                return new LogEntry
                {
                    ClientIP = ip
                };
            }
            else throw new Exception($"Failed to parse IP {clientIp}");
        }
    }
}
