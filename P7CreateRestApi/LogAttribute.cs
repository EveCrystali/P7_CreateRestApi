using PostSharp.Aspects;
using PostSharp.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace Dot.Net.WebApi
{
    [PSerializable]
    public class LogAspect : OnMethodBoundaryAspect
    {

        [NonSerialized]
        private ILogger<LogAspect> _logger;

        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public override void RuntimeInitialize(MethodBase method)
        {
            _logger = _loggerFactory.CreateLogger<LogAspect>();
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Entering {args.Method.Name}";
            _logger.LogInformation(messageLog);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Exiting {args.Method.Name}";
            _logger.LogInformation(messageLog);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"POSTSHARP: Exception in {args.Method.Name}: {args.Exception.Message}";
            _logger.LogError(messageLog);
        }
    }

    [PSerializable]
    public class LogApiCallAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private ILogger<LogApiCallAspect> _logger;

        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public override void RuntimeInitialize(MethodBase method)
        {
            _logger = _loggerFactory.CreateLogger<LogApiCallAspect>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var httpContext = (HttpContext)args.Instance.GetType()
                .GetProperty("HttpContext")
                ?.GetValue(args.Instance, null);

            if (httpContext != null)
            {
                var request = httpContext.Request;
                Console.WriteLine($"POSTSHARP: API call to {request.Path} with method {request.Method}");
            }
            else
            {
                Console.WriteLine("POSTSHARP: No HttpRequest argument found.");
            }
            Console.WriteLine($"POSTSHARP: Entering {args.Method.Name}");
        }


        public override void OnSuccess(MethodExecutionArgs args)
        {
            var httpContext = (HttpContext)args.Instance.GetType()
                .GetProperty("HttpContext")
                ?.GetValue(args.Instance, null);

            if (httpContext != null)
            {
                var response = httpContext.Response;
                string messageLog = $"API call succeeded: Status {response.StatusCode}";
                _logger.LogInformation(messageLog);

                if (args.ReturnValue is ObjectResult result)
                {
                    string messageLog2 = $"POSTSHARP: Response Body: {System.Text.Json.JsonSerializer.Serialize(result.Value)}";
                    _logger.LogInformation(messageLog2);
                }
            }
            else
            {
                _logger.LogWarning("POSTSHARP: No HttpContext found.");
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string messageLog = $"API call failed: {args.Exception.Message}";
            _logger.LogError(messageLog);
        }
    }
}