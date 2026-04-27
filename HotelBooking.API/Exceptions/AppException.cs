namespace HotelBooking.API.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; set; }
    protected AppException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }
}

// Exception classes
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404) { }
}

public class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message, 400) { }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message, 409) { }
}