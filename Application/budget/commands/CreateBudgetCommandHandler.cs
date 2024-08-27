using Application.exceptions;
using Application.mediator;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.budget.commands;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Guid>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Category> _categoryRepository;


    public CreateBudgetCommandHandler
        (
            IBudgetRepository budgetRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Category> categoryRepository
        )
    {
        _budgetRepository = budgetRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateBudgetCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);

        if (user is null || category is null)
        {
            throw new NotFoundException($"user {command.UserId} or category {command.CategoryId}");
        }

        var activeBudget = await _budgetRepository.GetActiveBudgetByUserAndCategory(command.UserId, command.CategoryId);

        if (activeBudget != null)
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
}