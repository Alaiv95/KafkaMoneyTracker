﻿using Application.mappers;
using Application.mediator;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Application.mediatorHandlers.auth;
using Application.kafka.producer;
using Application.kafka.consumer;
using Application.handlers.transactions;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BudgetSpecs>();

        services.AddScoped<BudgetMapper>();
        services.AddScoped<AuthMapper>();
        services.AddScoped<TransactionMapper>();

        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<GetBudgetListQueryHandler>();
        services.AddScoped<RegisterUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<CreateTransactionCommandHandler>();

        services.AddScoped<IEventsProducer, EventsProducer>();
        services.AddHostedService<TransactionsConsumer>();
        services.AddMediator();

        return services;
    }
}
