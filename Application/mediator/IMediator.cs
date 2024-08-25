namespace Application.mediator;

public interface IMediator
{
    public void RegisterCommand<TCommand, TResponse>(
        Type commandName, ICommandHandler<TCommand, TResponse> handler)
        where TCommand : ICommand;

    public Task<TResponse> HandleCommand<TCommand, TResponse>(TCommand command)
        where TCommand : ICommand;
}