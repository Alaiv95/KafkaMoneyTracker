using Application.tempCommands;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection collection)
    {
        collection.AddSingleton<IMediator>(_ =>
        {
            IMediator mediator = new Mediator();
            mediator.RegisterCommand(typeof(TempCommand), new TempCommandHandler());

            return mediator;
        });

        return collection;
    }
}