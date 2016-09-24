using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IISLP.Core.Models;

namespace IISLP.Core.Parsers
{
    // refer https://msdn.microsoft.com/en-us/library/ms525807(v=vs.90).aspx#Anchor_1
    public class IISParser : BaseParser
    {
        protected const string DELMITER = ", ";
        protected const int INDEX_CLIENT_IP = 0;

        protected override LogEntry ProcessLine(string line)
        {
            // this format is pretty poor. everything is delimited with a colon and there is no escaping-mechanism.
            // so it is possible to call /controller/aaa,bbb which would then be logged as:
            // 127.0.0.1, david, 9/23/2016, 15:42:02, W3SVC3, surface-david, 127.0.0.1, 1693, 979, 3729, 200, 0, POST, /controller/aaa,bbb, id=5,
            // Microsoft does not mention a white-space in it's specification.

            var values = line.Split(new string[] { DELMITER }, StringSplitOptions.None);

            if (values.Length > INDEX_CLIENT_IP)
            {
                return this.BuildLogEntry(values[INDEX_CLIENT_IP]);
            }
            else throw new Exception($"Line seems to be malformed.");
        }
    }
}
