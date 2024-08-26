namespace Application.mediator;

public interface IMediator
{
    public void RegisterCommand<TCommand, TResponse>(
        Type commandName, IRequestHandler<TCommand, TResponse> handler)
        where TCommand : IRequest;

    public Task<TResponse> HandleCommand<TCommand, TResponse>(TCommand command)
        where TCommand : IRequest;
}