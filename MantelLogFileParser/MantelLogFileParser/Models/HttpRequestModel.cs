using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MantelLogFileParser.Models
{
    public class HttpRequestModel
    {
        private const string datePattern = @"(?<=\[)(.*?)(?=\])";
        private const string subStringPattern = "(?<= \")(.*?)(?=\")";

        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public RequestUrl RequestUrl { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int Bytes { get; set; }
        public string UserAgent { get; set; }


        public HttpRequestModel CreateFrom(string logItem)
        {
            var logItemArray = logItem.Split(" ");
            Regex dateRegex = new Regex(datePattern);
            MatchCollection matched = dateRegex.Matches(logItem);

            Regex subStringRegex = new Regex(subStringPattern);
            MatchCollection substrings = subStringRegex.Matches(logItem);

            return new HttpRequestModel
            {
                IpAddress = logItemArray[0],
                UserName = logItemArray[2],
                Timestamp = CleanDate(matched.FirstOrDefault().ToString()),
                RequestUrl = new RequestUrl(substrings[0].ToString()),
                StatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), logItemArray[8], true),
                Bytes = int.TryParse(logItemArray[9], out int result) ? result : 0,
                UserAgent = substrings[2].ToString()
            };
        }

        private DateTime CleanDate(string timestamp)
        {
            var regex = new Regex(Regex.Escape(":"));
            return DateTime.Parse(regex.Replace(timestamp, " ", 1));
        }
    }

    public class RequestUrl
    {
        public string HttpMethod { get; set; }
        public string Uri { get; set; }
        public string HttpVersion { get; set; }

        public RequestUrl(string requestUrl)
        {
            if (!string.IsNullOrEmpty(requestUrl))
            {
                var requestParams = requestUrl.Split(' ').ToArray();

                HttpMethod = requestParams[0];
                Uri = requestParams[1];
                HttpVersion = requestParams[2];
            }
        }
    }
}