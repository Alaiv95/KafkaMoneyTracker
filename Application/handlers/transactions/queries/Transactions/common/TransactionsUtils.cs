using Application.Dtos;
using Domain.Entities.Transaction;

namespace Application.handlers.transactions.queries.Transactions.common;

internal static class TransactionsUtils
{
    internal static List<TransactionSummaryDto> GetTransactionsSummaryInfo(List<TransactionInfo>? transactions)
    {
        if (transactions is null)
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