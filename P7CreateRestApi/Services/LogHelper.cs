using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dot.Net.WebApi.Helpers
{
    public static class LogHelper
    {
        public static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.AddConsole());
        public static readonly string LogFilePath = Path.Combine("logs", $"Log-{DateTime.Now:yyyy-MM-dd}.txt");

        public static void EnsureLogDirectoryExists()
        {
            string logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(logDirectory) && logDirectory != null)
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void LogToFile(string message, ILogger logger)
        {
            try
            {
                File.AppendAllText(LogFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                string messageLog = $"LogHelper: Failed to write to log file: {ex.Message}";
                logger.LogError(messageLog);
            }
        }
    }
}
