using Application.exceptions;
using Application.mediator;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.budget.commands;

public class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Guid>
{
    private readonly IMoneyTrackerDbContext _dbContext;

    public CreateBudgetCommandHandler(IMoneyTrackerDbContext dbContext) => _dbContext = dbContext;

    public async Task<Guid> Handle(CreateBudgetCommand command)
    {
        // TODO - create repositories and move db actions there
        var entitiesNotFound = await VerifyCategoryAndUserExistsAsync(command.UserId, command.CategoryId);
        var isBudgetOfChosenCategoryExists = await CheckCategoryBudgetExistsAsync(command.UserId, command.CategoryId);

        if (entitiesNotFound)
        {
            throw new NotFoundException(command.UserId + ", " + command.CategoryId);
        }

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

    private async Task<bool> VerifyCategoryAndUserExistsAsync(Guid userId, Guid categoryId)
    {
        var errorIds = new HashSet<Guid>();

        var user = await _dbContext.Users.FindAsync(userId);
        var category = await _dbContext.Categories.FindAsync(categoryId);

        if (user is null)
        {
            errorIds.Add(userId);
        }
        
        if (category is null)
        {
            errorIds.Add(categoryId);
        }

        return errorIds.Any();
    }

    private async Task<bool> CheckCategoryBudgetExistsAsync(Guid userId, Guid categoryId)
    {
        var budgets = await _dbContext.Budgets.Where(budget =>
            budget.UserId == userId &&
            budget.CategoryId == categoryId &&
            budget.PeriodEnd >= DateTime.Now
        ).ToListAsync();

        return budgets.Any();
    }
}