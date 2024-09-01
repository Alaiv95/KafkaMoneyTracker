using Application.Dtos;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.commands.UpdateBudget;
using Application.handlers.budget.queries.GetBudgetList;
using AutoMapper;
using Domain.Models;

namespace Application.mappers;

public class BudgetMappingProfile : Profile
{
    public BudgetMappingProfile()
    {
        CreateMap<Budget, BudgetLookUpVm>();
        CreateMap<BudgetCreateDto, CreateBudgetCommand>();
        CreateMap<BaseFilterSearchDto, GetBudgetListQuery>();
        CreateMap<Budget, BudgetUpdateResponseDto>();
        CreateMap<BudgetUpdateRequestDto, UpdateBudgetCommand>();
    }
}