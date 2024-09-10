using Application.mediator.interfaces;
using Domain.Entities.Transaction;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactions;

public class GetUserTransactionsQuery : IRequest<List<TransactionInfo>>
{
    public Guid UserId { get; set; }

    public Guid? BudgetId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }
}