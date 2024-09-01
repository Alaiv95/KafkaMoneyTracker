using Application.mappers;
using Application.mediator;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;
using Application.kafka.producer;
using Application.kafka.consumer;
using Application.MailClient;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.queries.CheckSpentBudget;
using Application.handlers.budget.queries.GetBudgetList;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;
using Application.handlers.category.command.AddCategory;
using Application.handlers.category.queries;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.GetUserTransactions;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BudgetSpecs>();
        services.AddScoped<TransactionSpecs>();

        services.AddScoped<BudgetMapper>();
        services.AddScoped<AuthMapper>();
        services.AddScoped<TransactionMapper>();

        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<GetBudgetListQueryHandler>();
        services.AddScoped<RegisterUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<CreateTransactionCommandHandler>();
        services.AddScoped<CheckSpentBudgetQueryHandler>();
        services.AddScoped<CancelTransactionsCommandHandler>();
        services.AddScoped<GetUserTransactionQueryHandler>();
        services.AddScoped<GetCategoriesQueryHandler>();
        services.AddScoped<AddCategoryCommandHandler>();

        services.AddScoped<IEventsProducer, EventsProducer>();
        services.AddHostedService<TransactionsConsumer>();
        services.AddHostedService<BudgetExceededConsumer>();
        services.AddScoped<IMailClient, SmtpMailClient>();

        services.AddMediator();

        return services;
    }
}
