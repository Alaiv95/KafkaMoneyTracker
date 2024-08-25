using Application.exceptions;

namespace Application.mediator;

public class Mediator : IMediator
{
    public IDictionary<Type, Object> _commands = new Dictionary<Type, Object>();

    public void RegisterCommand<TCommand, TResponse>(Type commandName, ICommandHandler<TCommand, TResponse> handler)
        where TCommand : ICommand
    {
        var isCommandAdded = _commands.ContainsKey(commandName);

        if (isCommandAdded)
        {
            throw new CommandAlreadyRegisteredException(commandName.ToString());
        }

        _commands[commandName] = handler;
    }

    public async Task<TResponse> HandleCommand<TCommand, TResponse>(TCommand command)
        where TCommand : ICommand
    {
        var commandName = command.GetType();
        var isCommandAdded = _commands.TryGetValue(commandName, out var commandHandlerObject);

        if (!isCommandAdded)
        {
            throw new CommandNotRegisteredException(commandName.ToString());
        }

        if (commandHandlerObject is not ICommandHandler<TCommand, TResponse> commandHandler)
        {
            throw new ApplicationException($"{commandName} is not valid.");
        }

        return await commandHandler.Handle(command);
    }
}