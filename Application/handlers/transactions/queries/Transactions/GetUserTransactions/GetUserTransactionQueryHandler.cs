using Application.exceptions;
using Application.mediator.interfaces;
using AutoMapper;
using Core.common;
using Domain.Entities.Transaction;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactions;

public class GetUserTransactionQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, PaginationContainer<TransactionInfo>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _transactionsMapper;
    private readonly TransactionSpecs _spec;

    public GetUserTransactionQueryHandler(
        ITransactionRepository transactionRepository,
        IMapper mapper,
        TransactionSpecs spec
    )
    {
        _transactionRepository = transactionRepository;
        _transactionsMapper = mapper;
        _spec = spec;
    }

    public async Task<PaginationContainer<TransactionInfo>> Handle(GetUserTransactionsQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException(
                $"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}"
            );
        }

        var baseSearchDto = _transactionsMapper.Map<BaseBudgetSearchFilter>(query);
        var transactionsCount = await _transactionRepository.CountTransactionsAsync(_spec.Build(baseSearchDto));
        var totalPages = (int)Math.Ceiling(transactionsCount / (double)query.DisplayLimit);
        var transactions = await _transactionRepository
            .SearchWithIncludeAsync(
                _spec.Build(baseSearchDto),
                limit: query.DisplayLimit,
                page: query.PageNumber
            );

        return new PaginationContainer<TransactionInfo>
        {
            PageNumber = query.PageNumber,
            TotalPages = totalPages,
            Data = transactions
        };
    }
}