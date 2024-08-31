using Application.Dtos;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;

namespace Application.mappers;

public class TransactionMapper
{
    public CreateTransactionCommand DtoToCreateCommand(TransactionCreateDto dto, Guid userId)
    {
        return new CreateTransactionCommand
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            UserId = userId
        };
    }
    
    public CancelTransactionsCommand DtoToCancelCommand(BaseItemListDto dto, Guid userId)
    {
        return new CancelTransactionsCommand
        {
            TransactionIds = dto.ItemsIds,
            UserId = userId
        };
    }
}
