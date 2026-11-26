using System;
using Microsoft.AspNetCore.Diagnostics;
using TaskFlow.API.Exceptions.Response;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.API.Exceptions.Handler;

public sealed class GlobalExceptionHandler  : IExceptionHandler{  
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            ProjectNotFoundException =>
                (StatusCodes.Status404NotFound, "Projeto não encontrado."),

            ProjectForbiddenException =>
                (StatusCodes.Status403Forbidden, "Não tens permissão para realizar esta operação."),

            ArgumentException =>
                (StatusCodes.Status400BadRequest, exception.Message),

            InvalidOperationException =>
                (StatusCodes.Status400BadRequest, exception.Message),

            _ =>
                (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.")
        };

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "Erro não tratado durante o processamento do pedido.");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Pedido rejeitado: {Message}",
                exception.Message);
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Detail = statusCode == StatusCodes.Status500InternalServerError
                    ? null
                    : exception.Message,
                Timestamp = DateTimeOffset.UtcNow
            },
            cancellationToken);

        return true;
    }
}



