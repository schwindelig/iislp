using System.Net;

namespace IISLP.Core.Models
{
    public class ClientData
    {
        public IPAddress IP { get; set; }

        public IPHostEntry HostEntry { get; set; }

        public string FQDN
        {
            get
            {
                return this.HostEntry?.HostName ?? "N/A";
            }
        }

        public int RequestCount { get; set; }
    }
}