using Application.Dtos;
using Application.handlers.transactions.queries.Transactions.common;
using Application.mediator.interfaces;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Microsoft.IdentityModel.Tokens;
using Core.common;

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

        var transactions = await _transactionRepository.SearchWithIncludeAsync(_spec.Build(filter));

        if (transactions.IsNullOrEmpty())
        {
            return new List<TransactionSummaryDto>();
        }

        return TransactionsUtils.GetTransactionsSummaryInfo(transactions);
    }
}