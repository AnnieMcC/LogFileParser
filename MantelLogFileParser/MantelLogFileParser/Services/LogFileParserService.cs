using System;
using System.Collections.Generic;
using System.Linq;
using MantelLogFileParser.Models;

namespace MantelLogFileParser.Services
{
    public interface ILogFileParserService
    {
        LogFileData GetLogData();
    }

    public class LogFileParserService : ILogFileParserService
    {
        private readonly IDataFileService _dataFileService;

        public LogFileParserService(IDataFileService dataFileService)
        {
            _dataFileService = dataFileService;
        }

        public LogFileData GetLogData()
        {
            var logFileData = new LogFileData();

            try
            {
                var fileResult = _dataFileService.GetDataFromFile();

                if (fileResult != null && fileResult.Any())
                {
                    logFileData = ParseLogFileData(fileResult);
                }
            }
            catch (Exception ex)
            {

            }

            Console.WriteLine(logFileData);

            return logFileData;
        }

        public LogFileData ParseLogFileData(string[] fileResult)
        {
            var httpRequests = new List<HttpRequestModel>();

            foreach (var logItem in fileResult)
            {
                if (!string.IsNullOrEmpty(logItem))
                {
                    var httpRequestModel = new HttpRequestModel();
                    httpRequests.Add(httpRequestModel.CreateFrom(logItem));
                }
            }

            return new LogFileData { HttpRequests = httpRequests };
        }
    }
}
