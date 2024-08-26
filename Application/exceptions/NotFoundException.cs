namespace Application.exceptions;

public class NotFoundException : Exception, IApiException
{
    public NotFoundException(string id)
        : base($"Entities with id {id} not found") { }

    public int ErrorCode => 404;
}