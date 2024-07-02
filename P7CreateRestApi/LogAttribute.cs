using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Dot.Net.WebApi
{
    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private ILogger<LogAspect> _logger;

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public static readonly string _logFilePath = Path.Combine("logs", $"Log-{DateTime.Now:yyyy-MM-dd}.txt");

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
                string messageLog = $"Failed to write to log file: {ex.Message}";
                _logger.LogError(messageLog);
            }
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            _logger = _loggerFactory.CreateLogger<LogAspect>();
            EnsureLogDirectoryExists();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Exiting {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Exception in {args.Method.Name}: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogToFile(messageLog);
        }
    }

    [PSerializable]
    public class LogApiCallAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private ILogger<LogApiCallAspect> _logger;

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");

        private void EnsureLogDirectoryExists()
        {
            string? logDirectory = Path.GetDirectoryName(_logFilePath);
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
                string messageLog = $"Failed to write to log file: {ex.Message}";
                _logger.LogError(messageLog);
            }
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            _logger = _loggerFactory.CreateLogger<LogApiCallAspect>();
            EnsureLogDirectoryExists();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            HttpContext? httpContext = (HttpContext?)args.Instance.GetType()
                .GetProperty("HttpContext")
                ?.GetValue(args.Instance, null);

            if (httpContext != null)
            {
                HttpRequest request = httpContext.Request;
                string messageLog = $"API call to {request.Path} with method {request.Method}";
                _logger.LogInformation(messageLog);
                LogToFile(messageLog);
            }
            else
            {
                string messageLog = $"POSTSHARP: No HttpContext argument found.";
                _logger.LogInformation(messageLog);
                LogToFile(messageLog);
            }
            string messageLog2 = $"POSTSHARP: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog2);
            LogToFile(messageLog2);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            HttpContext? httpContext = (HttpContext?)args.Instance.GetType()
                .GetProperty("HttpContext")
                ?.GetValue(args.Instance, null);

            if (httpContext != null)
            {
                HttpResponse response = httpContext.Response;
                string messageLog = $"API call succeeded: Status {response.StatusCode}";
                _logger.LogInformation(messageLog);
                LogToFile(messageLog);

                if (args.ReturnValue is ObjectResult result)
                {
                    string messageLog2 = $"POSTSHARP: Response Body: {System.Text.Json.JsonSerializer.Serialize(result.Value)}";
                    _logger.LogInformation(messageLog2);
                    LogToFile(messageLog2);
                }
            }
            else
            {
                _logger.LogWarning("POSTSHARP: No HttpContext found.");
                LogToFile("POSTSHARP: No HttpContext found.");
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"API call failed: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogToFile(messageLog);
        }
    }
}