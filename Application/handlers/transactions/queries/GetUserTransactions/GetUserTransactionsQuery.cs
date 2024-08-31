using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.queries.GetUserTransactions;

public class GetUserTransactionsQuery : IRequest<List<TransactionLookupExtendedDto>>
{
    public Guid UserId { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public DateTime? DateFrom { get; set; }
    
    public DateTime? DateTo { get; set; }
}