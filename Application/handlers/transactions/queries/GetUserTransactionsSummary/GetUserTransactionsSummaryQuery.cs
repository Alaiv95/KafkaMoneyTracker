using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.queries.GetUserTransactionsSummary;

public class GetUserTransactionsSummaryQuery : IRequest<List<TransactionSummaryDto>>
{
    public Guid UserId { get; set; }
    
    public Guid BudgetId { get; set; }
}