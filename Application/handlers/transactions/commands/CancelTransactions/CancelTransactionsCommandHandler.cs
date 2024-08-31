using Application.Dtos;
using Application.mediator.interfaces;
using Infrastructure.Repositories;
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

        var transactionsDict = command.TransactionIds!.ToDictionary(k => k, v => v);
        var transactions = await _transactionRepository.GetByIdsAsync(transactionsDict);
        var currentUserTransactions = transactions
            .Where(t => t.UserId == command.UserId)
            .ToList();
        
        foreach (var transaction in currentUserTransactions)
        {
            transaction.IsActive = false;
        }
        
        await _transactionRepository.UpdateRangeAsync(currentUserTransactions);

        return currentUserTransactions.Select(t => new TransactionLookupDto
        {
            CategoryId = t.CategoryId,
            IsActive = t.IsActive,
            Amount = t.Amount
        }).ToList();
    }
}