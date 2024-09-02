using Application.Dtos;
using Application.mediator.interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Application.handlers.transactions.commands.CancelTransactions;

public class CancelTransactionsCommandHandler : IRequestHandler<CancelTransactionsCommand, List<TransactionLookupDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public CancelTransactionsCommandHandler(ITransactionRepository transactionRepository)
        => _transactionRepository = transactionRepository;
    
    public async Task<List<TransactionLookupDto>> Handle(CancelTransactionsCommand command)
    {
        if (command.TransactionIds.IsNullOrEmpty())
        {
            return new List<TransactionLookupDto>();
        }
        
        var transactions = await _transactionRepository.GetByIdsAsync(command.TransactionIds!);
        var currentUserTransactions = transactions
            .Where(t => t.UserId == command.UserId)
            .ToList();
        
        foreach (var transaction in currentUserTransactions)
        {
            transaction.Deactivate();
        }
        
        await _transactionRepository.UpdateRangeAsync(currentUserTransactions);

        return currentUserTransactions.Select(t => new TransactionLookupDto
        {
            BudgetId = t.BudgetId,
            IsActive = t.IsActive,
            Amount = t.Money.Amount
        }).ToList();
    }
}