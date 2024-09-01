using System.Linq.Expressions;
using Application.extentions;
using Application.handlers.budget.queries.GetBudgetList;
using Infrastructure.Models;


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
        return (budget) => (dateFrom == null) || dateFrom <= budget.CreatedAt.AddDays(budget.DurationInDays);
    }

    Expression<Func<Budget, bool>> WithDateGtOrEt(DateTime? dateTo)
    {
        return (budget) => (dateTo == null) || dateTo >= budget.CreatedAt;
    }

    Expression<Func<Budget, bool>> WithCategoryId(Guid? categoryId)
    {
        return (budget) => (categoryId == null || categoryId == Guid.Empty) || budget.CategoryId == categoryId;
    }
}
