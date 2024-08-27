namespace Application.exceptions;

public class UserAlreadyExistsException : Exception, IApiException
{

    public UserAlreadyExistsException(string? message) : base(message)
    {
    }

    public int ErrorCode => 400;
}