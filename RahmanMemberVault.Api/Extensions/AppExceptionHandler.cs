using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RahmanMemberVault.Api.Extensions
{
    // Global exception handler that formats error responses and generates tracking IDs for unexpected errors.
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
            // Decide status based on exception type
            int statusCode;
            if (exception is KeyNotFoundException) statusCode = StatusCodes.Status404NotFound;
            else if (exception is InvalidOperationException) statusCode = StatusCodes.Status400BadRequest;
            else statusCode = StatusCodes.Status500InternalServerError;

            // Generate tracking ID only for truly unexpected (500) errors
            string? trackingId = statusCode == StatusCodes.Status500InternalServerError
                ? Guid.NewGuid().ToString()
                : null;

            // Log appropriately
            if (statusCode == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Unexpected error occurred. Tracking ID: {TrackingId}", trackingId);
            else
                _logger.LogWarning(exception, "Handled exception ({StatusCode}): {Message}", statusCode, exception.Message);

            // Prepare response
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            // Build the body
            var response = new
            {
                error = exception.Message,
                trackingId,                                                    // null unless 500
                stackTrace = (_env.IsDevelopment() && statusCode == 500)      // only include stack on 500 in dev
                                ? exception.StackTrace
                                : null,
                statusCode
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions), cancellationToken);
            return true;
        }
    }
}
