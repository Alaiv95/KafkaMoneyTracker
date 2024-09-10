using Application.Dtos;
using Application.handlers.transactions.queries.Transactions.common;
using Application.mediator.interfaces;
using Core.common;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactionsSummary;

public class
    GetUserTransactionsSummaryQueryHandler : IRequestHandler<GetUserTransactionsSummaryQuery,
    List<TransactionSummaryDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly TransactionSpecs _spec;

    public GetUserTransactionsSummaryQueryHandler(
        ITransactionRepository transactionRepository,
        TransactionSpecs spec
    )
    {
        _transactionRepository = transactionRepository;
        _spec = spec;
    }

    public async Task<List<TransactionSummaryDto>> Handle(GetUserTransactionsSummaryQuery command)
    {
        var filter = new BaseBudgetSearchFilter
        {
            BudgetId = command.BudgetId,
            UserId = command.UserId
        };

        var paginatedTransactions = await _transactionRepository
            .SearchWithIncludeAsync(_spec.Build(filter));

        return TransactionsUtils.GetTransactionsSummaryInfo(paginatedTransactions);
    }
}