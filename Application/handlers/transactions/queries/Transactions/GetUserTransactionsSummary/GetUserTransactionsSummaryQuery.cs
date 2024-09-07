using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.queries.Transactions.GetUserTransactionsSummary;

public class GetUserTransactionsSummaryQuery : IRequest<PaginationContainer<TransactionSummaryDto>>
{
    public Guid UserId { get; set; }
    
    public Guid BudgetId { get; set; }
    
    public int PageNumber { get; set; }
    
    public int DisplayLimit { get; set; }
}