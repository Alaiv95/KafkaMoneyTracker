using Application.mediator.interfaces;
using Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Application.handlers.transactions.commands.CancelTransactions;

public class CancelTransactionsCommandHandler : IRequestHandler<CancelTransactionsCommand, List<Guid>>
{
    private readonly ITransactionRepository _transactionRepository;

    public CancelTransactionsCommandHandler(ITransactionRepository transactionRepository)
        => _transactionRepository = transactionRepository;
    
    public async Task<List<Guid>> Handle(CancelTransactionsCommand command)
    {
        if (command.TransactionIds.IsNullOrEmpty())
        {
            return new List<Guid>();
        }

        var transactionsDict = command.TransactionIds.ToDictionary(k => k, v => v);
        var deactivatedTransactions = await _transactionRepository.DeactivateTransactionsAsync(transactionsDict);

        return deactivatedTransactions.Select(t => t.Id).ToList();
    }
}