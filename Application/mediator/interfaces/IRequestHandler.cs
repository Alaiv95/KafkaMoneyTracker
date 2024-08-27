namespace Application.mediator.interfaces;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest command);
}