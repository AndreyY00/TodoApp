using System.Net;
using System.Text.Json;

namespace  TodoApp.API.Middleware
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
                    _logger.LogError(ex, "Ocurrió un error inesperado");
                    await HandleExceptionAsync(context, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = ex switch
                {
                    ArgumentNullException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new
                {
                    status = response.StatusCode,
                    message = ex.Message
                });

                return response.WriteAsync(result);
            }      
        }
}
