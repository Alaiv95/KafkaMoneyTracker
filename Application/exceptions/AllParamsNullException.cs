namespace Application.exceptions;

public class AllParamsNullException : Exception, IApiException
{
    public AllParamsNullException(string message) : base(message) { }
    public int ErrorCode { get; } = 400;
}