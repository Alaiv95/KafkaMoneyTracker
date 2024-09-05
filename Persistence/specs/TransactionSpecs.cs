using System.Linq.Expressions;
using Core.common;
using Infrastructure.extentions;
using Infrastructure.Models;

namespace Infrastructure.specs;

public class TransactionSpecs : ISpec<Transaction, BaseBudgetSearchFilter>
{
    public Expression<Func<Transaction, bool>> Build(BaseBudgetSearchFilter filter)
    {
        return WithUserId(filter.UserId)
            .And(WithDateLtOrEt(filter.DateFrom))
            .And(WithDateGtOrEt(filter.DateTo))
            .And(WithBudgetId(filter.BudgetId));
    }

    Expression<Func<Transaction, bool>> WithUserId(Guid userId)
    {
        return (transaction) => transaction.UserId == userId;
    }

    Expression<Func<Transaction, bool>> WithDateLtOrEt(DateTime? dateFrom)
    {
        return (transaction) => (dateFrom == null) || dateFrom <= transaction.CreatedAt;
    }

    Expression<Func<Transaction, bool>> WithDateGtOrEt(DateTime? dateTo)
    {
        return (transaction) => (dateTo == null) || dateTo >= transaction.CreatedAt;
    }

    Expression<Func<Transaction, bool>> WithBudgetId(Guid? budgetId)
    {
        return (transaction) => (budgetId == null || budgetId == Guid.Empty) || transaction.BudgetId == budgetId;
    }
}