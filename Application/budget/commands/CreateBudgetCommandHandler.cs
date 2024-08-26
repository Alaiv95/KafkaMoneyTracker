using Application.exceptions;
using Application.mediator;
using Domain;
using Infrastructure;

namespace Application.budget.commands;

public class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Guid>
{
    private readonly IMoneyTrackerDbContext _dbContext;

    public CreateBudgetCommandHandler(IMoneyTrackerDbContext dbContext) => _dbContext = dbContext;
    public async Task<Guid> Handle(CreateBudgetCommand command)
    {
        var isBudgetOfChosenCategoryExists = await CheckCategoryBudgetExistsAsync(command.UserId, command.CategoryId);

        if (isBudgetOfChosenCategoryExists)
        {
            throw new BudgetForCategoryAlreadyExistsException(command.CategoryId.ToString());
        }

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            CategoryId = command.CategoryId,
            BudgetLimit = command.BudgetLimit,
            CurrentBudget = 1,
            PeriodStart = command.PeriodStart,
            PeriodEnd = command.PeriodEnd,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        

        await _dbContext.Budgets.AddAsync(budget);
        await _dbContext.SaveChangesAsync(default);

        return budget.Id;
    }

    private async Task<bool> CheckCategoryBudgetExistsAsync(Guid userId, Guid categoryId)
    {
        var budgets = _dbContext.Budgets.Where(budget =>
            budget.UserId == userId &&
            budget.CategoryId == categoryId &&
            budget.PeriodEnd >= DateTime.Now
        ).ToList();

        var a = 2;

        return budgets.Any();
    }
}