using System.Collections.Generic;
using System.Linq;

namespace MantelLogFileParser.Models
{
    public class LogFileData
    {
        public List<HttpRequestModel> HttpRequests { get; set; }

        public override string ToString()
        {
            var uniqueIps = $"The number of unique IP addresses: {HttpRequests.Select(ht => ht.IpAddress).Distinct().Count()}\n";

            var query = HttpRequests.GroupBy(ht => ht.RequestUrl.Uri)
                .ToDictionary(ht => ht.Key, ht => ht.Count())
                .OrderByDescending(ht => ht.Value)
                .Take(3);
            var top3Urls = $"The top 3 URLs: {string.Join(", ", query)}\n";

            var query2 = HttpRequests.GroupBy(ht => ht.IpAddress)
                .ToDictionary(ht => ht.Key, ht => ht.Count())
                .OrderByDescending(ht => ht.Value)
                .Take(3);
            var top3ActiveIPs = $"The top 3 active IPs: {string.Join(", ", query2)}\n";

            return string.Concat(uniqueIps, top3Urls, top3ActiveIPs);
        }
    }
}