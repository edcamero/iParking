using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace iParking.API.Middleware
{
    /// <summary>
    /// Middleware para manejo global de excepciones.
    /// Aplica principio DRY centralizando el manejo de errores en toda la aplicación.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new
            {
                status = false,
                message = "Error interno del servidor",
                error = GetErrorMessage(exception),
                code = GetStatusCode(exception)
            };

            _logger.LogError(exception, "Error no manejado: {Message}", exception.Message);

            switch (exception)
            {
                case ArgumentException argEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse = new
                    {
                        status = false,
                        message = argEx.Message,
                        code = 400
                    };
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    errorResponse = new
                    {
                        status = false,
                        message = "No autorizado",
                        code = 401
                    };
                    break;

                case KeyNotFoundException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    errorResponse = new
                    {
                        status = false,
                        message = "Recurso no encontrado",
                        code = 404
                    };
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }

        private string GetErrorMessage(Exception ex)
        {
#if DEBUG
            return ex.Message + Environment.NewLine + ex.StackTrace;
#else
            return "Error interno del servidor";
#endif
        }

        private int GetStatusCode(Exception ex) => ex switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            KeyNotFoundException => 404,
            _ => 500
        };
    }

    /// <summary>
    /// Extensión para registrar el middleware.
    /// </summary>
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
