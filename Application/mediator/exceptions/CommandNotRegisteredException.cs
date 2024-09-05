namespace Application.mediator.exceptions;

public class CommandNotRegisteredException : Exception
{
    public CommandNotRegisteredException(string type)
        : base($"Command with {type} not registered!") { }
}