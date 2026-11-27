using System;

namespace TaskFlow.API.Exceptions.Response;

public sealed class ErrorResponse{

    public int StatusCode { get; init; }

    public string Message { get; init; } = string.Empty;

    public string? Detail { get; init; }

    public DateTimeOffset Timestamp { get; init; }
}



