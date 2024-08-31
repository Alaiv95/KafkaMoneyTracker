using Application.Dtos;
using Application.exceptions;
using Application.mappers;
using Application.mediator.interfaces;
using Application.specs;
using Infrastructure.Repositories;

namespace Application.handlers.transactions.queries.GetUserTransactions;

public class GetUserTransactionQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, List<TransactionLookupExtendedDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly TransactionMapper _transactionsMapper;
    private readonly TransactionSpecs _spec;

    public GetUserTransactionQueryHandler(
        ITransactionRepository transactionRepository,
        TransactionMapper mapper,
        TransactionSpecs spec
    )
    {
        _transactionRepository = transactionRepository;
        _transactionsMapper = mapper;
        _spec = spec;
    }

    public async Task<List<TransactionLookupExtendedDto>> Handle(GetUserTransactionsQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException(
                $"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}"
            );
        }

        var baseSearchDto = _transactionsMapper.QueryToBaseSearchDto(query);
        var transactions = await _transactionRepository
            .SearchWithIncludeAsync(_spec.Build(baseSearchDto));

        return _transactionsMapper.TransactionsToExtendedLookupDto(transactions);
    }
}