namespace Application.mediator.interfaces;

public interface IMediator
{
    public void Register<TCommand, TResponse>(
        Type commandName, IRequestHandler<TCommand, TResponse> handler)
        where TCommand : IRequest;

    public Task<TResponse> HandleRequest<TCommand, TResponse>(TCommand command)
        where TCommand : IRequest;
}