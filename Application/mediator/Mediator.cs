using Application.exceptions;
using Application.mediator.interfaces;

namespace Application.mediator;

public class Mediator : IMediator
{
    private IDictionary<Type, Object> _commands = new Dictionary<Type, Object>();

    public void Register<TCommand, TResponse>(Type commandName, IRequestHandler<TCommand, TResponse> handler)
        where TCommand : IRequest
    {
        var isCommandAdded = _commands.ContainsKey(commandName);

        if (isCommandAdded)
        {
            throw new CommandAlreadyRegisteredException(commandName.ToString());
        }

        _commands[commandName] = handler;
    }

    public async Task<TResponse> HandleRequest<TCommand, TResponse>(TCommand command)
        where TCommand : IRequest
    {
        var commandName = command.GetType();
        var isCommandAdded = _commands.TryGetValue(commandName, out var commandHandlerObject);

        if (!isCommandAdded)
        {
            throw new CommandNotRegisteredException(commandName.ToString());
        }

        if (commandHandlerObject is not IRequestHandler<TCommand, TResponse> commandHandler)
        {
            throw new ApplicationException($"{commandName} is not valid.");
        }

        return await commandHandler.Handle(command);
    }
}