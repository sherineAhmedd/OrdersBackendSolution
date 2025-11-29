using Orders.BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace Orders.API.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            if (ex is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = ex.Message;
            }
            else if (ex is InvalidOrderException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = ex.Message;
            }

            _logger.LogError(ex, "An exception occurred while processing {Method} {Path}",
                           context.Request.Method, context.Request.Path);
            context.Response.StatusCode = statusCode;
            var response = new
            {
                statusCode,
                message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
