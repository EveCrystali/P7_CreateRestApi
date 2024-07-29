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
            Stream originalBodyStream = context.Response.Body;
            using MemoryStream responseBody = new();
            context.Response.Body = responseBody;

            await _next(context);

            string path = context.Request.Path;
            string method = context.Request.Method;
            int statusCode = context.Response.StatusCode;
            string user = context.User?.Identity?.Name ?? "Anonymous";
            string messageLog = $"API call to {path} with method {method} by user {user} completed with Status {statusCode}";

            _logger.LogInformation(messageLog);
            LogHelper.LogToFile(messageLog, _logger);

            if (statusCode >= 400)
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string detailedErrorLog = $"Error response: {responseText}";
                _logger.LogError(detailedErrorLog);
                LogHelper.LogToFile(detailedErrorLog, _logger);
            }

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
