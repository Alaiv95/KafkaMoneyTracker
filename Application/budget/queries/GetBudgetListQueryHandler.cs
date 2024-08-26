using Application.mediator;
using Domain.Models;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Application.budget.queries;

public class GetBudgetListQueryHandler : IRequestHandler<GetBudgetListQuery, GetBudgetListDto>
{
    private readonly IBudgetRepository _budgetRepository;

    public GetBudgetListQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<GetBudgetListDto> Handle(GetBudgetListQuery command)
    {
        var budgetList = await _budgetRepository.SearchAsync(TempSpec(command));
        var budgetLookupList = budgetList.Select(EntityToLookUpDto).ToList();
        
        return new GetBudgetListDto { Budgets = budgetLookupList };
    }

    private Expression<Func<Budget, bool>> TempSpec(GetBudgetListQuery queryFilter)
    {
        return (budget) => budget.UserId == queryFilter.UserId && 
            ((budget.PeriodStart >= queryFilter.DateFrom && budget.PeriodStart <= queryFilter.DateTo) || 
            (budget.PeriodEnd <= queryFilter.DateTo && budget.PeriodEnd >= queryFilter.DateFrom));
    }

    private BudgetLookUpDto EntityToLookUpDto(Budget entity)
    {
        return new BudgetLookUpDto
        {
            BudgetLimit = entity.BudgetLimit,
            CategoryId = entity.CategoryId,
            PeriodEnd = entity.PeriodEnd,
            PeriodStart = entity.PeriodStart,
        };
    }
}
