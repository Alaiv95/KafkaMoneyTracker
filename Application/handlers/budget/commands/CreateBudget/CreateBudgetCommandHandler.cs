using Application.exceptions;
using Application.mediator.interfaces;
using Core.common;
using Domain.Entities;
using Domain.Entities.Budget;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Application.handlers.budget.commands.CreateBudget;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Guid>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IGenericRepository<Category, CategoryEntity> _categoryRepository;
    private readonly BudgetSpecs _budgetSpecs;

    public CreateBudgetCommandHandler
    (
        IBudgetRepository budgetRepository,
        IGenericRepository<Category, CategoryEntity> categoryRepository,
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

        var budget = BudgetEntity.Create(
            Limit.Create(command.BudgetLimit, command.DurationInDays),
            command.CategoryId,
            command.UserId
        );

        await _budgetRepository.AddAsync(budget);

        return budget.Id;
    }

    private async Task<bool> IsActiveBudgetOfCategoryExistsAsync(Guid userId, Guid categoryId)
    {
        var filter = new BaseCategorySearchFilter
        {
            UserId = userId,
            DateFrom = DateTime.Now,
            CategoryId = categoryId
        };

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(filter));

        return budgetList.Any();
    }
}