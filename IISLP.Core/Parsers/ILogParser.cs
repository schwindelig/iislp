using IISLP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISLP.Core.Parsers
{
    public interface ILogParser
    {
        IEnumerable<LogEntry> ParseLog(string path);
    }
}
