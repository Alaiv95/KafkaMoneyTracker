using Application.mediator.interfaces;
using Core.common;
using Domain.Entities.Transaction;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactions;

public class GetUserTransactionsQuery : IRequest<PaginationContainer<TransactionInfo>>
{
    public Guid UserId { get; set; }

    public Guid? BudgetId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public int PageNumber { get; set; }

    public int DisplayLimit { get; set; }
}