using Application.Dtos;
using Application.handlers.transactions.commands.CreateTransaction;

namespace Application.mappers;

public class TransactionMapper
{
    public CreateTransactionCommand DtoToCommand(TransactionCreateDto dto, Guid userId)
    {
        return new CreateTransactionCommand
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            UserId = userId
        };
    }
}
