namespace Application;

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest command);
}