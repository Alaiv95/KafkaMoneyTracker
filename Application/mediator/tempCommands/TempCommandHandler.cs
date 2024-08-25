namespace Application.mediator.tempCommands;

public class TempCommandHandler : ICommandHandler<TempCommand, string>
{
    public async Task<string> Handle(TempCommand command)
    {
        var id = command.Id.ToString();
        await Task.Delay(0);

        return id;
    }
}