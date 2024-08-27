namespace Application.exceptions;

public class ForbiddenException : Exception, IApiException
{
    public ForbiddenException(string message)
        : base(message) { }

    public int ErrorCode => 403;
}