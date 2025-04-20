using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RahmanMemberVault.Api.Extensions;

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
            // Determine if this is a known, user-friendly exception
            bool isKnown = exception is KeyNotFoundException;

            // For unexpected errors, generate a tracking ID
            string? trackingId = null;
            if (!isKnown)
            {
                trackingId = Guid.NewGuid().ToString();
            }

            // Log the exception with tracking ID for diagnostics
            if (isKnown)
            {
                _logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);
            }
            else
            {
                _logger.LogError(exception, "Unexpected error occurred. Tracking ID: {TrackingId}", trackingId);
            }

            // Prepare response
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

            string errorMessage;
            // Set error message based on exception type and environment
            if (isKnown)
            {
                errorMessage = exception.Message;
            }
            else if (_env.IsDevelopment())  
            {
                errorMessage = exception.Message;
            }
            else
            {
                errorMessage = "An unexpected error occurred. Please contact support with the tracking ID.";
            }

            // Build response payload
            var response = new
            {
                error = errorMessage,
                trackingId,
                stackTrace = (_env.IsDevelopment() && !isKnown) ? exception.StackTrace : null,
                statusCode = httpContext.Response.StatusCode
            };

            // Serialize response to JSON
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camel case for JSON properties
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull // Ignore null properties
            };

            var json = JsonSerializer.Serialize(response, jsonOptions);
            await httpContext.Response.WriteAsync(json, cancellationToken); // Write JSON response
            return true;
        }
    }
}