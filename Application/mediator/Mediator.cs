using Application.exceptions;
using Application.mediator.interfaces;

namespace Application.mediator;

public class Mediator : IMediator
{
    private IDictionary<Type, object> _commands = new Dictionary<Type, object>();

    public void Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        var commandName = typeof(TRequest);
        var isCommandAdded = _commands.ContainsKey(commandName);

        if (isCommandAdded)
        {
            throw new CommandAlreadyRegisteredException(commandName.ToString());
        }

        _commands[commandName] = handler;
    }

    public async Task<TResponse> HandleRequest<TRequest, TResponse>(TRequest command) where TRequest : IRequest<TResponse>
    {
        var commandName = typeof(TRequest);
        var isCommandAdded = _commands.TryGetValue(commandName, out var commandHandlerObject);

        if (!isCommandAdded)
        {
            throw new CommandNotRegisteredException(commandName.ToString());
        }

        if (commandHandlerObject is not IRequestHandler<TRequest, TResponse> commandHandler)
        {
            throw new ApplicationException($"{commandName} is not valid.");
        }

        return await commandHandler.Handle(command);
    }
}