namespace Application.mediator.exceptions;

public class CommandAlreadyRegisteredException : Exception
{
    public CommandAlreadyRegisteredException(string type)
        : base($"Command with {type} already registered!") { }
}