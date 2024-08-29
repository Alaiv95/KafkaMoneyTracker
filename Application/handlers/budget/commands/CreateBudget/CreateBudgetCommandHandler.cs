using Application.exceptions;
using Application.handlers.budget.queries.GetBudgetList;
using Application.mediator.interfaces;
using Application.specs;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.handlers.budget.commands.CreateBudget;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Guid>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly BudgetSpecs _budgetSpecs;

    public CreateBudgetCommandHandler
        (
            IBudgetRepository budgetRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Category> categoryRepository,
            BudgetSpecs budgetSpecs
        )
    {
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
        _budgetSpecs = budgetSpecs;
    }

    public async Task<Guid> Handle(CreateBudgetCommand command)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);

        if (category is null)
        {
            throw new NotFoundException($"category {command.CategoryId} not found");
        }

        var isBudgetExists = await IsActiveBudgetOfCategoryExistsAsync(command.UserId, command.CategoryId);

        if (isBudgetExists)
        {
            throw new BudgetForCategoryAlreadyExistsException(command.CategoryId.ToString());
        }

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            CategoryId = command.CategoryId,
            BudgetLimit = command.BudgetLimit,
            DurationInDays = command.DurationInDays,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        await _budgetRepository.AddAsync(budget);

        return budget.Id;
    }

    private async Task<bool> IsActiveBudgetOfCategoryExistsAsync(Guid userId, Guid categoryId)
    {
        var filter = new GetBudgetListQuery
        {
            UserId = userId,
            DateFrom = DateTime.Now,
            CategoryId = categoryId
        };

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(filter));

        return budgetList.Any();
    }

}