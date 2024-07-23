using System.Reflection;
using Dot.Net.WebApi.Helpers;
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
            // Ignore logging for constructors and static constructors
            if (args.Method is ConstructorInfo || args.Method.IsStatic && args.Method.Name == ".cctor")
            {
                return;
            }

            string messageLog = $"LogAspect: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            // Ignore logging for constructors and static constructors
            if (args.Method is ConstructorInfo || args.Method.IsStatic && args.Method.Name == ".cctor")
            {
                return;
            }

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
            // Ignore logging for constructors
            if (args.Method is ConstructorInfo)
            {
                return;
            }

            LogApiCall(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"LogApiCallAspect: API call failed: {args.Exception.Message}";
            _logger.LogError(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }

        private void LogApiCall(MethodExecutionArgs args)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                string messageLog2 = "LogApiCallAspect: _httpContextAccessor.HttpContext is null.";
                _logger.LogWarning(messageLog2);
                LogHelper.LogToFile(messageLog2, _logger);
                return;
            }

            HttpRequest request = _httpContextAccessor.HttpContext.Request;
            string messageLog = $"LogApiCallAspect: API call to {request.Path} with method {request.Method} entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }
    }
}
