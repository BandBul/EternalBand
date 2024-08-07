using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace EternalBAND.Win.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (!context.Response.HasStarted) 
                {
                    if (context.Response.StatusCode >= (int)HttpStatusCode.BadRequest &&
                        context.Response.StatusCode < (int)HttpStatusCode.InternalServerError)
                    {
                        await HandleClientErrorAsync(context);
                    }
                    else if (context.Response.StatusCode >= (int)HttpStatusCode.InternalServerError)
                    {
                        await HandleServerErrorAsync(context);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleClientErrorAsync(HttpContext context)
        {
            var result = "";
            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    result = JsonSerializer.Serialize(new { message = "Resource not found.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    _logger.LogWarning($"Unauthorized: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Unauthorized access.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.Forbidden:
                    _logger.LogWarning($"Forbidden: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Access forbidden.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.BadRequest:
                    _logger.LogWarning($"Bad Request: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Bad request.", statusCode = context.Response.StatusCode });
                    break;
                default:
                    _logger.LogWarning($"Client error occurred with status code: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "A client error occurred.", statusCode = context.Response.StatusCode });
                    break;
            }

            return context.Response.WriteAsync(result);
        }

        private Task HandleServerErrorAsync(HttpContext context)
        {
            string result = "";
            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.NotImplemented:
                    _logger.LogError($"Not Implemented: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Not implemented.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.InternalServerError:
                    _logger.LogError($"Internal Server Error: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Internal server error.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.BadGateway:
                    _logger.LogError($"Bad Gateway: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Bad gateway.", statusCode = context.Response.StatusCode });
                    break;
                case (int)HttpStatusCode.ServiceUnavailable:
                    _logger.LogError($"Service Unavailable: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "Service unavailable.", statusCode = context.Response.StatusCode });
                    break;
                default:
                    _logger.LogError($"Server error occurred with status code: {context.Response.StatusCode}");
                    result = JsonSerializer.Serialize(new { message = "A server error occurred.", statusCode = context.Response.StatusCode });
                    break;
            }

            return context.Response.WriteAsync(result);
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = new
            {
                message = "An internal server error has occurred.",
                detail = exception.Message
            }.ToString();

            return context.Response.WriteAsync(result);
        }
    }
}
