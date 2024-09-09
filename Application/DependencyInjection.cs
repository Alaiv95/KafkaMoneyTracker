using Application.Dtos;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.commands.UpdateBudget;
using Application.handlers.budget.queries.CheckSpentBudget;
using Application.handlers.budget.queries.GetBudgetList;
using Application.handlers.category.command.AddCategory;
using Application.handlers.category.command.DeleteCategory;
using Application.handlers.category.queries;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.Transactions.DownloadTransactionsSummary;
using Application.handlers.transactions.queries.Transactions.GetUserTransactions;
using Application.handlers.transactions.queries.Transactions.GetUserTransactionsSummary;
using Application.Jobs;
using Application.kafka.consumers;
using Application.kafka.producer;
using Application.mappers;
using Application.mediator;
using Application.utils.CurrencyConverter;
using Application.utils.FileInfoConfigurators;
using Domain.Entities.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(AuthMappingProfile),
            typeof(BudgetMappingProfile),
            typeof(TransactionMappingProfile),
            typeof(CategoryMappingProfile)
        );

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
        services.AddScoped<UpdateBudgetCommandHandler>();
        services.AddScoped<DeleteCategoryCommandHandler>();
        services.AddScoped<GetUserTransactionsSummaryQueryHandler>();
        services.AddScoped<DownloadTransactionsSummaryQueryHandler>();

        services.AddScoped<IEventsProducer, EventsProducer>();
        services.AddHostedService<TransactionsConsumer>();
        services.AddHostedService<BudgetExceededConsumer>();
        services.AddHostedService<UpdateExchangeRatesJob>();

        services.AddScoped<IConfigurator<List<TransactionSummaryDto>>, SummaryInfoConfigurator>();
        services.AddScoped<IConverter<Money>, MoneyConverter>();

        services.AddMediator();
    }
}