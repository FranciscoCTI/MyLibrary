using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Library.Core.Interfaces;
using Library.Global;

namespace Library.Services
{
    /// <summary>
    /// Register the exceptions thrown by the system in a defined way and location
    /// </summary>
    public class Logger: ILogger
    {
        private static readonly string ClientFolderName = MongoConstants.MongoDatabaseName;

        private static readonly object Lock = new object();
        private static ILogger? _instance;
        private readonly string _logFileExtension = LoggingConstants.LogFileDenomination;
        private readonly string _logDirectory;

        /// <summary>
        /// If <see cref="_instance"/>> is null, creates a singleton instance of this
        /// class, then return this instance.
        /// </summary>
        public static ILogger? Instance
        {
            get
            {
                if (_instance==null)
                {
                    lock (Lock)
                    {
                        var logDirectory = GetTempPath();
                        _instance = new Logger(logDirectory);
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Constructor to create an instance of <see cref="Logger"/>
        /// </summary>
        /// <param name="logDirectory"></param>
        public Logger(string logDirectory)
        {
            _logDirectory = logDirectory;
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// <summary>
        /// Return the path where the logs are going to be stored.
        /// </summary>
        /// <returns>A string with the path</returns>
        private static string GetTempPath()
        {
            var tempPath = Path.GetTempPath();

            var pattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

            var result = Regex.Replace(tempPath, pattern, "").Trim('\\');

            var logDirectory = $"{result}\\{ClientFolderName}\\";

            return logDirectory;
        }

        /// <inheritdoc />
        public void Log(string message)
        {
            var dateTime = DateTime.Now;
            var fileName = $"{dateTime:yy-MM-dd}{_logFileExtension}";
            var logPath = $@"{_logDirectory}{fileName}";
            using var writer = new StreamWriter(logPath, true);
            writer.WriteLine($"{dateTime}:{message}");
        }
    }
}
