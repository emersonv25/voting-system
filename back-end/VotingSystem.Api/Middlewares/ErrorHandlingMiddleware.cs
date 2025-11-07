using System.ComponentModel.DataAnnotations;
using System.Net;
using VotingSystem.Api.DTOs;

namespace VotingSystem.Api.Middlewares
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
            }
            catch (Exception ex)
            {
                // Aqui, você pode capturar as exceções de maneira genérica e formatá-las
                ErrorResponseDTO errorResponse = ex switch
                {
                    ValidationException _ => new ErrorResponseDTO(400, "Validation failed", new[] { ex.Message }),
                    ArgumentNullException _ => new ErrorResponseDTO(400, "Required argument is missing", new[] { ex.Message }),
                    KeyNotFoundException _ => new ErrorResponseDTO(404, "Resource not found", new[] { ex.Message }),
                    UnauthorizedAccessException _ => new ErrorResponseDTO(401, "Unauthorized access", new[] { ex.Message }),
                    TimeoutException _ => new ErrorResponseDTO(504, "The operation timed out", new[] { ex.Message }),
                    _ => new ErrorResponseDTO(500, "An unexpected error occurred", new[] { ex.Message })
                };

                _logger.LogError(ex, "An error occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = errorResponse.StatusCode;

                // Retorna a resposta formatada
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

    }

}
