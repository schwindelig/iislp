using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IISLP.Core.Models;
using System.Text.RegularExpressions;

namespace IISLP.Core.Parsers
{
    // refer: 
    //      https://msdn.microsoft.com/en-us/library/ms525807(v=vs.90).aspx#Anchor_2
    //      http://www.loganalyzer.net/log-analyzer/apache-common-log.html
    public class NCSAParser : BaseParser
    {
        protected const char DELIMITER = ' ';
        protected const int INDEX_CLIENT_IP = 0;

        protected override LogEntry ProcessLine(string line)
        {
            // actually, we would have to work with some regex to parse the line, but since we only need the client's IP
            // and the format is fixed, we can simply take the first element in the array.
            // the following shows a line in a NSCA Formatted log from IIS:
            // 127.0.0.1 - david [23/Sep/2016:16:27:00 +0200] "POST /Kategorie/Index_Read HTTP/1.1" 200 737

            var split = line.Split(DELIMITER);

            if (split.Length > INDEX_CLIENT_IP)
            {
                return this.BuildLogEntry(split[INDEX_CLIENT_IP]);
            }
            else throw new Exception($"Line seems to be malformed");
        }
    }
}
