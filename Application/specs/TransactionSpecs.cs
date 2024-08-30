using Domain.Models;
using System.Linq.Expressions;
using Application.extentions;
using Application.Dtos;


namespace Application.specs;

public class TransactionSpecs : ISpec<Transaction, BaseSearchDto>
{
    public Expression<Func<Transaction, bool>> Build(BaseSearchDto filter)
    {
        return WithUserId(filter.UserId)
            .And(WithDateLtOrEt(filter.DateFrom))
            .And(WithDateGtOrEt(filter.DateTo))
            .And(WithCategoryId(filter.CategoryId));
    }
    
    Expression<Func<Transaction, bool>> WithUserId(Guid userId)
    {
        return (budget) => budget.UserId == userId;
    }

    Expression<Func<Transaction, bool>> WithDateLtOrEt(DateTime? dateFrom)
    {
        return (budget) => (dateFrom == null) || dateFrom <= budget.CreatedAt;
    }

    Expression<Func<Transaction, bool>> WithDateGtOrEt(DateTime? dateTo)
    {
        return (budget) => (dateTo == null) || dateTo >= budget.CreatedAt;
    }

    Expression<Func<Transaction, bool>> WithCategoryId(Guid? categoryId)
    {
        return (budget) => (categoryId == null || categoryId == Guid.Empty) || budget.CategoryId == categoryId;
    }
}
