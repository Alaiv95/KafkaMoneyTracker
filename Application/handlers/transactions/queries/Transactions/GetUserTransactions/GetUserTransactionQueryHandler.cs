﻿using Application.Dtos;
using Application.exceptions;
using Application.mediator.interfaces;
using AutoMapper;
using Domain.Entities.Transaction;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Core.common;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactions;

public class GetUserTransactionQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, List<TransactionInfo>>
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

    public async Task<List<TransactionInfo>> Handle(GetUserTransactionsQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException(
                $"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}"
            );
        }
        
        var baseSearchDto = _transactionsMapper.Map<BaseBudgetSearchFilter>(query);
        
        var transactions = await _transactionRepository
            .SearchWithIncludeAsync(_spec.Build(baseSearchDto));

        return transactions;
    }
}