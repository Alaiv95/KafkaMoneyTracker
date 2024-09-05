using System.Linq.Expressions;
using Core.common;
using Infrastructure.extentions;
using Infrastructure.Models;

namespace Infrastructure.specs;

public class BudgetSpecs : ISpec<Budget, BaseCategorySearchFilter>
{
    public Expression<Func<Budget, bool>> Build(BaseCategorySearchFilter filter)
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