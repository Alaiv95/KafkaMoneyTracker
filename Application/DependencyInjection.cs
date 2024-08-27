﻿using Application.mappers;
using Application.mediator.interfaces;
using Application.mediator;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Application.mediatorHandlers.auth;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BudgetSpecs>();
        services.AddScoped<BudgetMapper>();
        services.AddScoped<AuthMapper>();
        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<GetBudgetListQueryHandler>();
        services.AddScoped<RegisterUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();

        services.AddScoped<IMediator>(provider =>
        {
            IMediator mediator = new Mediator();
            var createBudgetCommandHandler = provider.GetRequiredService<CreateBudgetCommandHandler>();
            var getBudgetQueryHandler = provider.GetRequiredService<GetBudgetListQueryHandler>();
            var registerHandler = provider.GetRequiredService<RegisterUserCommandHandler>();
            var loginHandler = provider.GetRequiredService<LoginCommandHandler>();

            mediator.Register(typeof(CreateBudgetCommand), createBudgetCommandHandler);
            mediator.Register(typeof(GetBudgetListQuery), getBudgetQueryHandler);
            mediator.Register(typeof(RegisterUserCommand), registerHandler);
            mediator.Register(typeof(LoginCommand), loginHandler);

            return mediator;
        });

        return services;
    }
}
