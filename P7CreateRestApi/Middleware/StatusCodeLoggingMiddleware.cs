using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dot.Net.WebApi.Helpers;

namespace Dot.Net.WebApi.Middleware
{
    public class StatusCodeLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<StatusCodeLoggingMiddleware> _logger;

        public StatusCodeLoggingMiddleware(RequestDelegate next, ILogger<StatusCodeLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            LogHelper.EnsureLogDirectoryExists();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            string path = context.Request.Path;
            string method = context.Request.Method;
            int statusCode = context.Response.StatusCode;
            string messageLog = $"API call to {path} with method {method} completed with Status {statusCode}";

            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);
        }
    }
}
