using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.commands.CancelTransactions;

public class CancelTransactionsCommand : IRequest<List<TransactionLookupDto>>
{
    public List<Guid>? TransactionIds { get; set; }
}