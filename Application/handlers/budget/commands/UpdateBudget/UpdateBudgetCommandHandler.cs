using Application.Dtos;
using Application.exceptions;
using Application.handlers.budget.queries.GetBudgetList;
using Application.mediator.interfaces;
using Application.specs;
using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Application.handlers.budget.commands.UpdateBudget;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, BudgetUpdateResponseDto>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly BudgetSpecs _budgetSpecs;
    private readonly IMapper _mapper;

    public UpdateBudgetCommandHandler
    (
        IBudgetRepository budgetRepository,
        BudgetSpecs budgetSpecs,
        IMapper mapper
    )
    {
        _budgetRepository = budgetRepository;
        _budgetSpecs = budgetSpecs;
        _mapper = mapper;
    }
    
    public async Task<BudgetUpdateResponseDto> Handle(UpdateBudgetCommand command)
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

        budget.BudgetLimit = command.BudgetLimit ?? budget.BudgetLimit;
        budget.DurationInDays = command.DurationInDays ?? budget.DurationInDays;
        budget.UpdatedAt = DateTime.Now;

        await _budgetRepository.UpdateOne(budget);

        return _mapper.Map<BudgetUpdateResponseDto>(budget);
    }
    
    private async Task<Budget?> FindActiveBudget(Guid userId, Guid categoryId)
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