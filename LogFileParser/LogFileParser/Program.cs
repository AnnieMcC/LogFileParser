using System;
using LogFileParser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogFileParser
{
    class Program
    {
        const string fileName = "programming-task-example-data.log";

        private static void Main(string[] args)
        {
           
            //configure services
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IDataFileService>(new DataFileService(fileName))
                .AddSingleton<ILogFileParserService>(x =>
                    new LogFileParserService(x.GetRequiredService<IDataFileService>()))
                .BuildServiceProvider();


            serviceProvider.GetService<ILoggerFactory>();
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            Console.WriteLine("Starting application...");

            Console.WriteLine($"Parsing file: {fileName}");
            var fileParser = serviceProvider.GetService<ILogFileParserService>();
            fileParser.GetLogData();

            Console.WriteLine("File parse complete");
        }
    }
}
