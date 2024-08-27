using Application.Dtos;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Domain.Models;

namespace Application.mappers;

public class BudgetMapper
{
    public BudgetLookUpDto EntityToDto(Budget entity)
    {
        return new BudgetLookUpDto
        {
            BudgetLimit = entity.BudgetLimit,
            CategoryId = entity.CategoryId,
            DurationInDays = entity.DurationInDays,
            CreatedAt = entity.CreatedAt,
        };
    }

    public CreateBudgetCommand DtoToCommand(BudgetCreateDto dto)
    {
        return new CreateBudgetCommand
        {
            BudgetLimit = dto.BudgetLimit,
            DurationInDays = dto.DurationInDays,
            CategoryId = dto.CategoryId,
            UserId = dto.UserId
        };
    }
}
