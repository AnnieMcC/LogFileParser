using System;
using Xunit;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Net;
using MantelLogFileParser.Services;

namespace LogFileParser.Tests
{
    public class UnitTest
    {
        private ILogFileParserService logFileParserService { get; set; }
        private IDataFileService dataFileService { get; set; }

        public UnitTest()
        {
            //Assembly thisAssembly = Assembly.GetExecutingAssembly();
        }

        [Fact]
        public void TestDataFileServiceReturnsData()
        {
            dataFileService = new DataFileService("test_data_10.log");
            var data = dataFileService.GetDataFromFile();

            Assert.NotEmpty(data);
            Assert.Equal(10, data.GetLength(0));
        }

        //[Fact]
        //public void TestDataFileServiceFileNotFound()
        //{
        //    dataFileService = new DataFileService("rubbish.log");
        //    var data = dataFileService.GetDataFromFile();

        //    Assert.Empty(data);
        //}

        //[Fact]
        //public void TestDataFileServiceReturnsEmptyArray()
        //{
        //    dataFileService = new DataFileService("test_data_empty.log");
        //    var data = dataFileService.GetDataFromFile();

        //    Assert.Empty(data);
        //}

        [Fact]
        public void TestParsedData()
        {
            dataFileService = new DataFileService("test_data_10.log");
            logFileParserService = new LogFileParserService(dataFileService);

            var data = logFileParserService.GetLogData();

            var firstLog = data.HttpRequests.First();
            var lastLog = data.HttpRequests.Last();
            var adminLog = data.HttpRequests.FirstOrDefault(req => string.Equals(req.UserName, "admin", StringComparison.OrdinalIgnoreCase));

            Assert.Equal("50.112.00.11", adminLog.IpAddress);
            Assert.Equal(HttpStatusCode.Moved, lastLog.StatusCode);
            Assert.Equal("Monday, 09 July 2018", firstLog.Timestamp.ToLongDateString());
            Assert.Equal("18:10:38", firstLog.Timestamp.ToLongTimeString());
        }

        [Fact]
        public void TestDataIsClean()
        {
            dataFileService = new DataFileService("test_data_10.log");
            logFileParserService = new LogFileParserService(dataFileService);

            var data = logFileParserService.GetLogData();

            var defaultLog = data.HttpRequests.FirstOrDefault(h => h.UserAgent.Contains("junk extra"));
            Assert.Null(defaultLog);
        }

        [Fact]
        public void TestTopIPAddress()
        {
            dataFileService = new DataFileService("test_data_10.log");
            logFileParserService = new LogFileParserService(dataFileService);

            var data = logFileParserService.GetLogData();

            Assert.Contains("The top 3 active IPs: [168.41.191.40, 2]", data.ToString());
        }

        [Fact]
        public void TestUniqueUrls()
        {
            dataFileService = new DataFileService("test_data_10.log");
            logFileParserService = new LogFileParserService(dataFileService);

            var data = logFileParserService.GetLogData();

            Assert.StartsWith("The number of unique IP addresses: 8", data.ToString());
        }
    }
}
