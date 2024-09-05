namespace Application.mediator.interfaces;

public interface IMediator
{
    public void Register<TCommand, TResponse>(
        IRequestHandler<TCommand, TResponse> handler)
        where TCommand : IRequest<TResponse>;

    public Task<TResponse> HandleRequest<TResponse>(IRequest<TResponse> command);
}