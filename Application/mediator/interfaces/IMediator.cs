namespace Application.mediator.interfaces;

public interface IMediator
{
    public void Register<TCommand, TResponse>(
        IRequestHandler<TCommand, TResponse> handler)
        where TCommand : IRequest<TResponse>;

    public Task<TResponse> HandleRequest<TRequest, TResponse>(TRequest command) where TRequest : IRequest<TResponse>;
}