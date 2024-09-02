﻿using Application.Dtos;
using Application.exceptions;
using Application.mappers;
using Application.mediator.interfaces;
using Application.specs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.transactions.queries.GetUserTransactions;

public class GetUserTransactionQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, List<TransactionLookupExtendedDto>>
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

    public async Task<List<TransactionLookupExtendedDto>> Handle(GetUserTransactionsQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException(
                $"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}"
            );
        }
        
        var baseSearchDto = _transactionsMapper.Map<BaseSearchDto>(query);
        
        var transactions = await _transactionRepository
            .SearchWithIncludeAsync(_spec.Build(baseSearchDto));

        return _transactionsMapper.Map<List<TransactionLookupExtendedDto>>(transactions);
    }
}