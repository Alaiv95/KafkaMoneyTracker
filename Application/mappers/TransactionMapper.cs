using Application.Dtos;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.GetUserTransactions;
using Domain.Models;

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
    
    public GetUserTransactionsQuery DtoToGetUserTransactionsQuery(BaseFilterSearchDto dto, Guid userId)
    {
        return new GetUserTransactionsQuery()
        {
            CategoryId = dto.CategoryId,
            DateTo = dto.DateTo,
            DateFrom = dto.DateFrom,
            UserId = userId
        };
    }
    
    public List<TransactionLookupExtendedDto> TransactionsToExtendedLookupDto(List<Transaction> transactions)
    {
        return transactions.Select(t =>
            new TransactionLookupExtendedDto
            {
                Amount = t.Amount,
                IsActive = t.IsActive,
                Category = new CategoryLookupDto
                {
                    CategoryType = t.Category.CategoryType,
                    Id = t.Category.Id
                }
            }
        ).ToList();
    }
    
    public BaseSearchDto QueryToBaseSearchDto(GetUserTransactionsQuery query)
    {
        return new BaseSearchDto
        {
            CategoryId = query.CategoryId,
            UserId = query.UserId,
            DateFrom = query.DateFrom,
            DateTo = query.DateTo
        };
    }
}
