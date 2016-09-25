using IISLP.Core.Parsers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IISLP.Web.Models
{
    public class UploadFilesVM
    {
        public ICollection<IFormFile> Files { get; set; }

        public LogFormat Format { get; set; }
    }

    public class ResultVm
    {
        public ICollection<ResultLogVm> Logs { get; set; }
    }

    public class ResultLogVm
    {
        public string File { get; set; }

        public IEnumerable<ResultClientVm> Clients { get; set; }
    }

    public class ResultClientVm
    {
        public string IP { get; set; }

        public string FQDN { get; set; }

        public int RequestCount { get; set; }
    }
}