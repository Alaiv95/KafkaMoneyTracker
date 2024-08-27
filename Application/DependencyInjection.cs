using Application.budget.commands;
using Application.budget.queries;
using Application.mappers;
using Application.mediator.interfaces;
using Application.mediator;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BudgetSpecs>();
        services.AddScoped<BudgetMapper>();
        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<GetBudgetListQueryHandler>();

        services.AddScoped<IMediator>(provider =>
        {
            IMediator mediator = new Mediator();
            var createBudgetCommandHandler = provider.GetRequiredService<CreateBudgetCommandHandler>();
            var getBudgetQueryHandler = provider.GetRequiredService<GetBudgetListQueryHandler>();

            mediator.Register(typeof(CreateBudgetCommand), createBudgetCommandHandler);
            mediator.Register(typeof(GetBudgetListQuery), getBudgetQueryHandler);

            return mediator;
        });

        return services;
    }
}
