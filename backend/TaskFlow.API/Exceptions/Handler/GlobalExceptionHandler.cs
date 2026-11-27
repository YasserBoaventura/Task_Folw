
using Microsoft.AspNetCore.Diagnostics;
using TaskFlow.API.Exceptions.Response;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.API.Exceptions.Handler;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken){  
        var (statusCode, message) = exception switch
        {
            ProjectNotFoundException =>
                (404, "Projeto não encontrado."),

            ProjectForbiddenException =>
                (403, "Não tens permissão para realizar esta operação."),

            ArgumentException =>
                (400, exception.Message),

            InvalidOperationException =>
                (400, exception.Message),

            _ =>
                (500, "Ocorreu um erro interno no servidor.")
        };

        if (statusCode >= 500)
        {
            _logger.LogError(exception, "Erro interno.");
        }
        else
        {
            _logger.LogWarning(exception, "Pedido rejeitado.");
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Detail = statusCode == 500 ? null : exception.Message,
                Timestamp = DateTimeOffset.UtcNow
            },
            cancellationToken);

        return true;
    }
}
