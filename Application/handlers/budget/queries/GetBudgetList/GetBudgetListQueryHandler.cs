using Application.exceptions;
using Application.mediator.interfaces;
using AutoMapper;
using Core.common;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

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
            throw new DateFromCantBeLessThenDateToException(
                $"DateFrom {query.DateFrom} can't be less then DateTo {query.DateTo}");
        }

        var filter = new BaseCategorySearchFilter
        {
            UserId = query.UserId,
            DateFrom = query.DateFrom,
            DateTo = query.DateTo,
            CategoryId = query.CategoryId
        };

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpecs.Build(filter));
        var budgetLookupList = _budgetMapper.Map<List<BudgetLookUpVm>>(budgetList);

        return new BudgetListVm { Budgets = budgetLookupList };
    }
}