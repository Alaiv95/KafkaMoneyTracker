using Application.mediator.interfaces;

namespace Application.mediator;

interface IHandlerWrapper
{
}

abstract class HandlerWrapperBase<TResponse> : IHandlerWrapper
{
    public abstract Task<TResponse> Handle(IRequest<TResponse> command);
}

class HandlerWrapper<TRequest, TResponse> : HandlerWrapperBase<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _handler;

    public HandlerWrapper(IRequestHandler<TRequest, TResponse> handler) => _handler = handler;

    public override async Task<TResponse> Handle(IRequest<TResponse> command) => 
        await _handler.Handle((TRequest)command);
}
