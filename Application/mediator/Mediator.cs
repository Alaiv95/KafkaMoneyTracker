using Application.exceptions;
using Application.mediator.interfaces;
using Serilog;

namespace Application.mediator;

public class Mediator : IMediator
{
    private IDictionary<Type, IHandlerWrapper> _commands = new Dictionary<Type, IHandlerWrapper>();

    public void Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        var commandName = typeof(TRequest);
        var isCommandAdded = _commands.ContainsKey(commandName);

        if (isCommandAdded)
        {
            Log.Error($"Attempet to add already registered command - {commandName}");
            throw new CommandAlreadyRegisteredException(commandName.ToString());
        }

        _commands[commandName] = new HandlerWrapper<TRequest, TResponse>(handler);
    }


    public async Task<TResponse> HandleRequest<TResponse>(IRequest<TResponse> command)
    {
        var commandName = command.GetType();
        var isCommandAdded = _commands.TryGetValue(commandName, out var commandHandlerObject);

        if (!isCommandAdded)
        {
            Log.Error($"Attempt to use not registered command - {commandName}");
            throw new CommandNotRegisteredException(commandName.ToString());
        }

        if (commandHandlerObject is not HandlerWrapperBase<TResponse> handlerCommand)
        {
            Log.Error($"Invalid command passed to mediator- {commandName}");
            throw new ApplicationException("Invalid command");
        }
        
        Log.Information($"Handle request for {commandName} -> {command}");

        return await handlerCommand.Handle(command);
    }
}