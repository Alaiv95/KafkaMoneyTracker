﻿using Application.mappers;
using Application.mediator.interfaces;
using Application.mediator;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Application.mediatorHandlers.auth;
using Application.handlers.transactions;
using Application.handlers.budget.queries;

namespace Application.mediator;

public static class DependencyInjection
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator>(provider =>
        {
            IMediator mediator = new Mediator();
            var createBudgetCommandHandler = provider.GetRequiredService<CreateBudgetCommandHandler>();
            var getBudgetQueryHandler = provider.GetRequiredService<GetBudgetListQueryHandler>();
            var registerHandler = provider.GetRequiredService<RegisterUserCommandHandler>();
            var loginHandler = provider.GetRequiredService<LoginCommandHandler>();
            var transactionCreateHandler = provider.GetRequiredService<CreateTransactionCommandHandler>();
            var checkBudgetQueryHandler = provider.GetRequiredService<CheckSpentBudgetQueryHandler>();

            mediator.Register(createBudgetCommandHandler);
            mediator.Register(getBudgetQueryHandler);
            mediator.Register(registerHandler);
            mediator.Register(loginHandler);
            mediator.Register(transactionCreateHandler);
            mediator.Register(checkBudgetQueryHandler);

            return mediator;
        });
    }
}
