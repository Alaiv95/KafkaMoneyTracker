namespace Application.exceptions;

public class DateFromCantBeLessThenDateToException : Exception, IApiException
{
    public DateFromCantBeLessThenDateToException(string message) : base(message) { }

    public int ErrorCode => 400;
}
