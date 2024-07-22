using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dot.Net.WebApi;

namespace Dot.Net.WebApi.Middleware
{
    public class StatusCodeLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        // private readonly ILogger<StatusCodeLoggingMiddleware> _logger;

        public StatusCodeLoggingMiddleware(RequestDelegate next, ILogger<LogAspect> logger)
        {
            _next = next;
            _logger = logger;
        }

        private ILogger<LogAspect> _logger;

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public static readonly string _logFilePath = Path.Combine("logs", $"Log-{DateTime.Now:yyyy-MM-dd}.txt");

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            string path = context.Request.Path;
            string method = context.Request.Method;
            int statusCode = context.Response.StatusCode;
            string messageLog = $"API call to {path} with method {method} completed with Status {statusCode}";

            _logger.LogInformation(messageLog);
            LogToFile(messageLog);
        }

        private void EnsureLogDirectoryExists()
        {
            string logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory) && logDirectory != null)
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                string messageLog = $"LogAspect: Failed to write to log file: {ex.Message}";
                _logger.LogError(messageLog);
            }
        }
    }
}