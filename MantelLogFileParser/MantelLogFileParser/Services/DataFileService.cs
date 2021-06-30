using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MantelLogFileParser.Services
{
    public interface IDataFileService
    {
        string[] GetDataFromFile();
    }

    public class DataFileService : IDataFileService
    {
        private string _dataFilePath { get; }

        public DataFileService(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public string[] GetDataFromFile()
        {
            try
            {
                //Assembly thisAssembly = Assembly.GetExecutingAssembly();
                //string path = "LogFileParser.Tests.Data";
                //var x = new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + _dataFilePath));

                var path = Path.GetRelativePath(Directory.GetCurrentDirectory(), _dataFilePath);

                string result;
                if (File.Exists(path))
                {
                    using (StreamReader reader = File.OpenText(path))
                    {
                        Console.WriteLine("Opened file.");
                        result = reader.ReadToEnd();
                        Console.WriteLine($"Contains: {result.Count()} bytes");

                        if (!string.IsNullOrEmpty(result))
                        {
                            var lineArray = result.Split("\r\n");
                            return lineArray.Where(l => !string.IsNullOrEmpty(l)).ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new string[] { };
        }
    }
}
