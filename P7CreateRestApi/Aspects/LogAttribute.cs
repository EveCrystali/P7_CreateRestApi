using System.Reflection;
using Dot.Net.WebApi.Helpers;
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

        public override void RuntimeInitialize(MethodBase method)
        {
            _logger = LogHelper.LoggerFactory.CreateLogger<LogAspect>();
            LogHelper.EnsureLogDirectoryExists();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            string messageLog = $"LogAspect: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string messageLog = $"LogAspect: Exiting {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"LogAspect: Exception in {args.Method.Name}: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }
    }

    [PSerializable]
    public class LogApiCallAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private ILogger<LogApiCallAspect> _logger;

        [NonSerialized]
        private IHttpContextAccessor _httpContextAccessor;

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

            LogHelper.EnsureLogDirectoryExists();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            LogMethodEntry(args);

            if (_httpContextAccessor.HttpContext == null)
            {
                string messageLog = "LogApiCallAspect: _httpContextAccessor.HttpContext is null.";
                _logger.LogWarning(messageLog);
                LogHelper.LogToFile(messageLog, _logger);
                return;
            }

            HttpRequest request = _httpContextAccessor.HttpContext.Request;
            string messageLog2 = $"LogApiCallAspect: API call to {request.Path} with method {request.Method}";
            _logger.LogInformation(messageLog2);
            LogHelper.LogToFile(messageLog2, _logger);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"LogApiCallAspect: API call failed: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        private void LogMethodEntry(MethodExecutionArgs args)
        {
            string messageLog = $"LogApiCallAspect: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        private void LogApiResponse(MethodExecutionArgs args)
        {
            if (_logger == null || _httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            int statusCode = httpContext.Response.StatusCode;

            string messageLog = $"LogApiCallAspect: API call to {httpContext.Request.Path} with method {httpContext.Request.Method} completed with Status {statusCode}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);

            if (args.ReturnValue is ObjectResult result)
            {
                string messageLog2 = $"LogApiCallAspect: Response Body: {System.Text.Json.JsonSerializer.Serialize(result.Value)}";
                _logger.LogInformation(messageLog2);
                LogHelper.LogToFile(messageLog2, _logger);
            }
            else if (args.ReturnValue is IActionResult actionResult)
            {
                string messageLog2 = $"LogApiCallAspect: Response Type: {actionResult.GetType().Name}";
                _logger.LogInformation(messageLog2);
                LogHelper.LogToFile(messageLog2, _logger);
            }
        }
    }
}