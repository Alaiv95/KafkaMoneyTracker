﻿using Application.handlers.auth.commands.Login;
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
using Application.mediator.interfaces;
using Microsoft.Extensions.DependencyInjection;

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
            var cancelTransactionCommandHandler = provider.GetRequiredService<CancelTransactionsCommandHandler>();
            var getUserTransactionQueryHandler = provider.GetRequiredService<GetUserTransactionQueryHandler>();
            var getCategoriesQueryHandler = provider.GetRequiredService<GetCategoriesQueryHandler>();
            var addCategoryCommandHandler = provider.GetRequiredService<AddCategoryCommandHandler>();
            var updateBudgetCommandHandler = provider.GetRequiredService<UpdateBudgetCommandHandler>();
            var deleteCategoryCommandHandler = provider.GetRequiredService<DeleteCategoryCommandHandler>();
            var getUserTransactionsSummaryQueryHandler = provider.GetRequiredService<GetUserTransactionsSummaryQueryHandler>();
            var downloadTransactionsSummaryQueryHandler = provider.GetRequiredService<DownloadTransactionsSummaryQueryHandler>();

            mediator.Register(createBudgetCommandHandler);
            mediator.Register(getBudgetQueryHandler);
            mediator.Register(registerHandler);
            mediator.Register(loginHandler);
            mediator.Register(transactionCreateHandler);
            mediator.Register(checkBudgetQueryHandler);
            mediator.Register(cancelTransactionCommandHandler);
            mediator.Register(getUserTransactionQueryHandler);
            mediator.Register(getCategoriesQueryHandler);
            mediator.Register(addCategoryCommandHandler);
            mediator.Register(updateBudgetCommandHandler);
            mediator.Register(deleteCategoryCommandHandler);
            mediator.Register(getUserTransactionsSummaryQueryHandler);
            mediator.Register(downloadTransactionsSummaryQueryHandler);

            return mediator;
        });
    }
}
