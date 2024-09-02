using Application.Dtos;
using Application.exceptions;
using Application.mediator.interfaces;
using Application.specs;
using AutoMapper;
using Infrastructure.Repositories.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Application.handlers.transactions.queries.GetUserTransactionsSummary;

public class GetUserTransactionsSummaryQueryHandler : IRequestHandler<GetUserTransactionsSummaryQuery, List<TransactionSummaryDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _transactionsMapper;
    private readonly TransactionSpecs _spec;
    private readonly IBudgetRepository _budgetRepository;

    public GetUserTransactionsSummaryQueryHandler(
        ITransactionRepository transactionRepository,
        IBudgetRepository budgetRepository,
        IMapper mapper,
        TransactionSpecs spec
    )
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _transactionsMapper = mapper;
        _spec = spec;
    }

    public async Task<List<TransactionSummaryDto>> Handle(GetUserTransactionsSummaryQuery command)
    {
        var filter = new BaseSearchDto
        {
            BudgetId = command.BudgetId,
            UserId = command.UserId
        };
        
        var transactions = await _transactionRepository.SearchWithIncludeAsync(_spec.Build(filter));

        if (transactions.IsNullOrEmpty())
        {
            return new List<TransactionSummaryDto>();
        }

        return transactions
            .GroupBy(t => t.Budget.CategoryName)
            .Select(g => new TransactionSummaryDto
            {
                CategoryName = g.Key,
                Income = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                Expenses = g.Where(t => t.Amount < 0).Sum(t => t.Amount)
            }).ToList();
    }
}