namespace Application.exceptions;

public class CategoryAlreadyExistsException : Exception, IApiException
{
    public CategoryAlreadyExistsException(string name) : base(name) { }
    public int ErrorCode => 400;
}