using Application.exceptions;
using Application.mappers;
using Application.mediator.interfaces;
using Application.specs;
using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.budget.queries.GetBudgetList;

public class GetBudgetListQueryHandler : IRequestHandler<GetBudgetListQuery, BudgetListVm>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly BudgetSpecs _budgetSpecs;
    private readonly IMapper _budgetMapper;

    public GetBudgetListQueryHandler(IBudgetRepository budgetRepository, BudgetSpecs budgetSpecs, IMapper mapper)
    {
        _budgetRepository = budgetRepository;
        _budgetSpecs = budgetSpecs;
        _budgetMapper = mapper;
    }

    public async Task<BudgetListVm> Handle(GetBudgetListQuery query)
    {
        if (query.DateFrom > query.DateTo)
        {
            throw new DateFromCantBeLessThenDateToException($"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}");
        }

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(query));
        var budgetLookupList = _budgetMapper.Map<List<BudgetLookUpVm>>(budgetList);

        return new BudgetListVm { Budgets = budgetLookupList };
    }
}
