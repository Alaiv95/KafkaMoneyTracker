namespace Application.mediator;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest command);
}