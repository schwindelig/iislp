using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISLP.Core.Models
{
    public class LogEntry
    {
        public string ClientIP { get; set; }

        public string Url { get; set; }

        public DateTime Date { get; set; }
    }
}
