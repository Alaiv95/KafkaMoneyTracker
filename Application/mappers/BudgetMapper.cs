using Application.Dtos;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Domain.Models;

namespace Application.mappers;

public class BudgetMapper
{
    public BudgetLookUpVm EntityToDto(Budget entity)
    {
        return new BudgetLookUpVm
        {
            BudgetLimit = entity.BudgetLimit,
            CategoryId = entity.CategoryId,
            DurationInDays = entity.DurationInDays,
            CreatedAt = entity.CreatedAt,
        };
    }

    public CreateBudgetCommand DtoToCommand(BudgetCreateDto dto, Guid userId)
    {
        return new CreateBudgetCommand
        {
            BudgetLimit = dto.BudgetLimit,
            DurationInDays = dto.DurationInDays,
            CategoryId = dto.CategoryId,
            UserId = userId
        };
    }

    public GetBudgetListQuery DtoToQuery(BudgetGetListDto dto, Guid userId)
    {
        return new GetBudgetListQuery
        {
            DateFrom = dto.DateFrom,
            DateTo = dto.DateTo,
            CategoryId = dto.CategoryId,
            UserId = userId
        };
    }
}
