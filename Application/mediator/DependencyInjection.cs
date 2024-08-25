using Application.budget.commands;
using Microsoft.Extensions.DependencyInjection;

namespace Application.mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection collection)
    {
        collection.AddSingleton<CreateBudgetCommandHandler>();
        collection.AddSingleton<IMediator>(provider =>
        {
            IMediator mediator = new Mediator();
            var createBudgetCommandHandler = provider.GetRequiredService<CreateBudgetCommandHandler>();

            mediator.RegisterCommand(typeof(CreateBudgetCommand), createBudgetCommandHandler);

            return mediator;
        });

        return collection;
    }
}