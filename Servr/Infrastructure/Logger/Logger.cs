using System.IO;

using Servr.Domain.Interface;

namespace Servr.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath = "app.log")
        {
            using StreamWriter writer = new StreamWriter(logFilePath, append: false); // Overwrite/reset the file on startup
            _logFilePath = logFilePath;
        }

        public void Log(LogLevel level, string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] - {message}";
            using StreamWriter writer = new StreamWriter(_logFilePath, append: true);
            writer.WriteLine(logMessage);
        }
    }
}
