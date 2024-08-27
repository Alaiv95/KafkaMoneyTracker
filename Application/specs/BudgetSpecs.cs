using Application.budget.queries;
using Domain.Models;
using System.Linq.Expressions;
using Application.extentions;


namespace Application.specs;

public class BudgetSpecs : ISpec<Budget, GetBudgetListQuery>
{
    public Expression<Func<Budget, bool>> Build(GetBudgetListQuery filter)
    {
        return WithUserId(filter.UserId)
            .Also(WithDateLtOrEt(filter.DateFrom))
            .Also(WithDateGtOrEt(filter.DateTo));
    }
    
    Expression<Func<Budget, bool>> WithUserId(Guid userId)
    {
        return (budget) => budget.UserId == userId;
    }

    Expression<Func<Budget, bool>> WithDateLtOrEt(DateTime? dateFrom)
    {
        return (budget) => (dateFrom != null && dateFrom.HasValue) ? dateFrom <= budget.CreatedAt.AddDays(budget.DurationInDays) : true;
    }

    Expression<Func<Budget, bool>> WithDateGtOrEt(DateTime? dateTo)
    {
        return (budget) => (dateTo != null && dateTo.HasValue) ? dateTo >= budget.CreatedAt : true;
    }
}
