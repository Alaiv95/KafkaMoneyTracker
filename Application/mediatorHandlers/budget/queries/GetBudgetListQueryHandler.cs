﻿using Application.exceptions;
using Application.mappers;
using Application.mediator.interfaces;
using Application.specs;
using Infrastructure.Repositories;

namespace Application.mediatorHandlers.budget.queries;

public class GetBudgetListQueryHandler : IRequestHandler<GetBudgetListQuery, GetBudgetListVm>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly BudgetSpecs _budgetSpecs;
    private readonly BudgetMapper _budgetMapper;

    public GetBudgetListQueryHandler(IBudgetRepository budgetRepository, BudgetSpecs budgetSpecs, BudgetMapper budgetMapper)
    {
        _budgetRepository = budgetRepository;
        _budgetSpecs = budgetSpecs;
        _budgetMapper = budgetMapper;
    }

    public async Task<GetBudgetListVm> Handle(GetBudgetListQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException($"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}");
        }

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(query));
        var budgetLookupList = budgetList.Select(_budgetMapper.EntityToDto).ToList();

        return new GetBudgetListVm { Budgets = budgetLookupList };
    }
}
