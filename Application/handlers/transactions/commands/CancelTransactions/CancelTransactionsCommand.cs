using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.commands.CancelTransactions;

public class CancelTransactionsCommand : IRequest<List<TransactionLookupDto>>
{
    public Guid UserId { get; set; }
    public List<Guid>? TransactionIds { get; set; }
}