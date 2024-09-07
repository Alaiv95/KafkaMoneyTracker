using Application.Dtos;
using Application.handlers.transactions.queries.Transactions.common;
using Application.mediator.interfaces;
using Core.common;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactionsSummary;

public class
    GetUserTransactionsSummaryQueryHandler : IRequestHandler<GetUserTransactionsSummaryQuery,
    PaginationContainer<TransactionSummaryDto>>
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

    public async Task<PaginationContainer<TransactionSummaryDto>> Handle(GetUserTransactionsSummaryQuery command)
    {
        var filter = new BaseBudgetSearchFilter
        {
            BudgetId = command.BudgetId,
            UserId = command.UserId
        };

        var spec = _spec.Build(filter);
        var transactionsCount = await _transactionRepository.CountTransactionsAsync(spec);
        var totalPages = (int)Math.Ceiling(transactionsCount / (double)command.DisplayLimit);
        var paginatedTransactions = await _transactionRepository.SearchWithIncludeAsync(
            spec,
            page: command.PageNumber,
            limit: command.DisplayLimit
        );

        return new PaginationContainer<TransactionSummaryDto>
        {
            PageNumber = command.PageNumber,
            TotalPages = totalPages,
            Data = TransactionsUtils.GetTransactionsSummaryInfo(paginatedTransactions)
        };
    }
}