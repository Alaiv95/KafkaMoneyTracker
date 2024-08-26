namespace Application.exceptions;

public class NotFoundException : Exception, IApiException
{
    public NotFoundException(string message)
        : base($"Entities {message} not found") { }

    public int ErrorCode => 404;
}