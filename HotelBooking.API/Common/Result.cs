namespace HotelBooking.API.Common;

public enum ErrorType
{
    NotFound,
    Validation,
    Conflict,
    Unauthorized
}

public record Error(ErrorType Type, string Message);

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);

    public static Result NotFound(string message) => Failure(new Error(ErrorType.NotFound, message));
    public static Result ValidationError(string message) => Failure(new Error(ErrorType.Validation, message));
    public static Result ConflictError(string message) => Failure(new Error(ErrorType.Conflict, message));
    public static Result Unauthorized(string message) => Failure(new Error(ErrorType.Unauthorized, message));
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error)
    {
        Value = default;
    }

    public static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(Error error) => new(error);

    public new static Result<T> NotFound(string message) => Failure(new Error(ErrorType.NotFound, message));
    public new static Result<T> ValidationError(string message) => Failure(new Error(ErrorType.Validation, message));
    public new static Result<T> ConflictError(string message) => Failure(new Error(ErrorType.Conflict, message));
    public new static Result<T> Unauthorized(string message) => Failure(new Error(ErrorType.Unauthorized, message));
}
