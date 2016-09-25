using IISLP.Core.Models;
using System;

namespace IISLP.Core.Parsers
{
    // refer: https://www.w3.org/TR/WD-logfile.html
    public class W3CParser : BaseParser
    {
        protected const string PREFIX_COMMENT = "#";
        protected const string FIELDS_DIRECTIVE = PREFIX_COMMENT + "Fields:";
        protected const char FIELDS_DELIMITER = ' ';
        protected const char ENTRY_DELIMITER = ' ';
        protected const string FIELD_CLIENT_IP = "c-ip";

        protected int? FieldIndexClientIp = null;

        protected override LogEntry ProcessLine(string line)
        {
            // comment
            if (line.StartsWith(PREFIX_COMMENT))
            {
                // check if this line contains the fields directive
                if (line.StartsWith(FIELDS_DIRECTIVE))
                {
                    // get the fields specified in the directive
                    string directive = line.Remove(0, FIELDS_DIRECTIVE.Length).Trim();
                    string[] fields = directive.Split(FIELDS_DELIMITER);

                    // get the index of the client-ip field
                    int fieldIndexClientIp = Array.IndexOf(fields, FIELD_CLIENT_IP);
                    if (this.FieldIndexClientIp != -1)
                    {
                        this.FieldIndexClientIp = fieldIndexClientIp;
                    }
                    else
                    {
                        this.FieldIndexClientIp = null;
                        throw new Exception($"Required Field {FIELD_CLIENT_IP} was not found in field directive.");
                    }
                }
            }
            // log entry
            else
            {
                // check if the client ip field index is specified
                if (this.FieldIndexClientIp.HasValue)
                {
                    string[] values = line.Split(ENTRY_DELIMITER);
                    if (values.Length > this.FieldIndexClientIp)
                    {
                        string ipStr = values[this.FieldIndexClientIp.Value];
                        return this.BuildLogEntry(ipStr);
                    }
                    else throw new Exception($"Line doesn't seem to contain the required information.");
                }
                else throw new Exception($"No fields defined. Is this a valid W3C Log File?");
            }

            return null;
        }
    }
}