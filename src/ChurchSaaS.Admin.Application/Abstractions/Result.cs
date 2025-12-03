namespace ChurchSaaS.Admin.Application.Abstractions;

public sealed class Result<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }
    public T? Data { get; init; }

    private Result(bool success, T? data, string? message, IReadOnlyList<string>? errors)
    {
        Success = success;
        Data = data;
        Message = message;
        Errors = errors;
    }

    public static Result<T> Ok(T data, string? message = null)
        => new(true, data, message, null);

    public static Result<T> Fail(string message, IEnumerable<string>? errors = null)
        => new(false, default, message, errors?.ToArray());
}
