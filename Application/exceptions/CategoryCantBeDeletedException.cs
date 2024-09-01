namespace Application.exceptions;

public class CategoryCantBeDeletedException : Exception, IApiException
{
    public CategoryCantBeDeletedException(string name) : base(name) { }
    public int ErrorCode { get; } = 400;
}