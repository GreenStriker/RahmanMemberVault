using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace RahmanMemberVault.Api.Extensions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<AppExceptionHandler> _logger;

        public AppExceptionHandler(IHostEnvironment env, ILogger<AppExceptionHandler> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred. Please contact support.",
                stackTrace = _env.IsDevelopment() ? exception.StackTrace : null,
                statusCode = httpContext.Response.StatusCode
            };


            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, jsonOptions);

            await httpContext.Response.WriteAsync(json, cancellationToken);
            return true;
        }
    }
}
