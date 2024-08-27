namespace Application.exceptions;

public class NotFoundException : Exception, IApiException
{
    public NotFoundException(string message)
        : base(message) { }

    public int ErrorCode => 404;
}