namespace Application.exceptions;

public class ItemsIdsCantBeNullException : Exception, IApiException
{
    public int ErrorCode { get; } = 400;
}