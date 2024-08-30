using System.ComponentModel.DataAnnotations;
using Application.mediator.interfaces;

namespace Application.handlers.transactions.commands.CancelTransactions;

public class CancelTransactionsCommand : IRequest<List<Guid>>
{
    public List<Guid> TransactionIds { get; set; }
}