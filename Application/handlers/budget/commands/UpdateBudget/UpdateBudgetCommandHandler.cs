using Application.Dtos;
using Application.exceptions;
using Application.handlers.budget.queries.GetBudgetList;
using Application.mediator.interfaces;
using Application.specs;
using AutoMapper;
using Domain.Entities.Budget;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.budget.commands.UpdateBudget;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Limit>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly BudgetSpecs _budgetSpecs;

    public UpdateBudgetCommandHandler
    (
        IBudgetRepository budgetRepository,
        BudgetSpecs budgetSpecs
    )
    {
        _budgetRepository = budgetRepository;
        _budgetSpecs = budgetSpecs;
    }
    
    public async Task<Limit> Handle(UpdateBudgetCommand command)
    {
        if (command.BudgetLimit is null && command.DurationInDays is null) 
        {
            throw new AllParamsNullException($"Both BudgetLimit and DurationInDays can't be null");
        }
        
        var budget = await FindActiveBudget(command.UserId, command.CategoryId);

        if (budget is null)
        {
            throw new NotFoundException($"Budget for category with id {command.CategoryId} not found");
        }

        var newLimit = command.BudgetLimit ?? budget.BudgetLimit.Amount;
        var newDuration = command.DurationInDays ?? budget.BudgetLimit.Duration;

        var limitValue = Limit.Create(newLimit, newDuration);
        
        await _budgetRepository.UpdateOne(budget.Id, limitValue);

        return limitValue;
    }
    
    private async Task<BudgetEntity?> FindActiveBudget(Guid userId, Guid categoryId)
    {
        var filter = new GetBudgetListQuery
        {
            UserId = userId,
            DateFrom = DateTime.Now,
            CategoryId = categoryId
        };

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(filter));

        return budgetList.FirstOrDefault();
    }
}