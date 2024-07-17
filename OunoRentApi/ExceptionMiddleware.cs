using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OunoRentApi
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }

            // Eğer kullanıcı kimliği doğrulanmamışsa
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var response = new
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = "Login required.",
                    Detailed = "You need to log in to access this resource."
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                return; // İşlemi sonlandır
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Detailed = exception.Message
            };

            if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Resource not found.",
                    Detailed = exception.Message
                };
            }
            else if (exception is ArgumentException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Invalid request.",
                    Detailed = exception.Message
                };
            }
            else if (exception is UnauthorizedAccessException) // 500 hatası
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred.",
                    Detailed = "Internal server error."
                };
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}