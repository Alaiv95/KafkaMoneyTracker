using Domain.Models;
using System.Linq.Expressions;
using Application.extentions;
using Application.mediatorHandlers.budget.queries;


namespace Application.specs;

public class BudgetSpecs : ISpec<Budget, GetBudgetListQuery>
{
    public Expression<Func<Budget, bool>> Build(GetBudgetListQuery filter)
    {
        return WithUserId(filter.UserId)
            .And(WithDateLtOrEt(filter.DateFrom))
            .And(WithDateGtOrEt(filter.DateTo))
            .And(WithCategoryId(filter.CategoryId));
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

    Expression<Func<Budget, bool>> WithCategoryId(Guid? categoryId)
    {
        return (budget) => (categoryId != null && categoryId != Guid.Empty) ? budget.CategoryId == categoryId : true;
    }
}
