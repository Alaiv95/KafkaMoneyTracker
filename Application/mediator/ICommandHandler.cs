namespace Application.mediator;

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest command);
}