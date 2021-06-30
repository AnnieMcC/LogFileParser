using System;
using System.Collections.Generic;

namespace LogFileParser.Models
{
    public class LogFileDataSummary
    {
        public int UniqueIps { get; set; }

        public List<string> Top3Urls { get; set; }

        public List<Version> Top3Ips { get; set; }
    }
}
