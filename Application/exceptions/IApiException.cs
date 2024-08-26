namespace Application.exceptions;

public interface IApiException
{
    public int ErrorCode { get; }
}
