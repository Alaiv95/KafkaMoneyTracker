using Application.budget.queries;
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
}
