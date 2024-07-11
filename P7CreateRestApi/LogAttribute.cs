using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace Dot.Net.WebApi
{
    public static class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    throw new InvalidOperationException("ServiceProvider has not been initialized.");
                }
                return _serviceProvider;
            }
            set
            {
                if (_serviceProvider != null)
                {
                    throw new InvalidOperationException("ServiceProvider is already set.");
                }
                _serviceProvider = value;
            }
        }

        public static T GetService<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }

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
                string messageLog = $"LogAspect: Failed to write to log file: {ex.Message}";
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
            string messageLog = $"LogAspect: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string messageLog = $"LogAspect: Exiting {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"LogAspect: Exception in {args.Method.Name}: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogToFile(messageLog);
        }
    }

    [PSerializable]
    public class LogApiCallAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private ILogger<LogApiCallAspect> _logger;

        [NonSerialized]
        private IHttpContextAccessor _httpContextAccessor;

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static readonly string _logFilePath = Path.Combine("logs", $"Log-{DateTime.Now:yyyy-MM-dd}.txt");

        public void EnsureLogDirectoryExists()
        {
            string? logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory) && logDirectory != null)
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                string messageLog = $"LogApiCallAspect: Failed to write to log file: {ex.Message}";
                _logger.LogError(messageLog);
            }
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            if (ServiceProviderHelper.ServiceProvider == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: ServiceProvider is not initialized");
            }

            _logger = ServiceProviderHelper.GetService<ILogger<LogApiCallAspect>>();
            if (_logger == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: Logger is not available");
            }

            _httpContextAccessor = ServiceProviderHelper.GetService<IHttpContextAccessor>();
            if (_httpContextAccessor == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: HttpContextAccessor is not available");
            }
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: Logger is not available");
            }

            _logger.LogWarning("Entering method {0}", args.Method.Name);

            if (_httpContextAccessor.HttpContext == null)
            {
                string messageLog3 = "LogApiCallAspect: _httpContextAccessor.HttpContext is null.";
                _logger.LogWarning(messageLog3);
                LogToFile(messageLog3);
                return;
            }

            HttpRequest request = _httpContextAccessor.HttpContext.Request;
            string messageLog = $"LogApiCallAspect: API call to {request.Path} with method {request.Method}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);

            string messageLog2 = $"LogApiCallAspect: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog2);
            LogToFile(messageLog2);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: Logger is not available");
            }

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            HttpResponse response = httpContext.Response;
            string messageLog = $"LogApiCallAspect: API call succeeded: Status {response.StatusCode}";
            _logger.LogInformation(messageLog);
            LogToFile(messageLog);

            if (args.ReturnValue is ObjectResult result)
            {
                string messageLog2 = $"LogApiCallAspect: Response Body: {System.Text.Json.JsonSerializer.Serialize(result.Value)}";
                _logger.LogInformation(messageLog2);
                LogToFile(messageLog2);
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("LogApiCallAspect: Logger is not available");
            }

            string messageLog = $"LogApiCallAspect: API call failed: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogToFile(messageLog);
        }
    }
}