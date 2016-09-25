using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IISLP.Core.Models
{
    public class ClientData
    {
        public IPAddress IP { get; set; }

        public IPHostEntry HostEntry { get; set; }

        public string FQDN { get; set; }

        public int RequestCount { get; set; }
    }
}
