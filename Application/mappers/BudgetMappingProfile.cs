using Application.Dtos;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.commands.UpdateBudget;
using Application.handlers.budget.queries.GetBudgetList;
using AutoMapper;
using Domain.Entities.Budget;
using Infrastructure.Models;
using Core.common;

namespace Application.mappers;

public class BudgetMappingProfile : Profile
{
    public BudgetMappingProfile()
    {
        CreateMap<Budget, BudgetLookUpVm>();
        CreateMap<BudgetCreateDto, CreateBudgetCommand>();
        CreateMap<BaseSearchDto, GetBudgetListQuery>();
        CreateMap<Budget, BudgetUpdateResponseDto>();
        CreateMap<BudgetUpdateRequestDto, UpdateBudgetCommand>();
        CreateMap<Budget, BudgetLookUpDto>();
        CreateMap<Budget, BudgetInfo>()
            .ForPath(dest => dest.CategoryName, opt => opt.MapFrom(model => model.Category.CategoryName));

        CreateMap<BudgetEntity, Budget>()
            .ForPath(model => model.BudgetLimit, opt => opt.MapFrom(ent => ent.BudgetLimit.Amount))
            .ForPath(model => model.DurationInDays, opt => opt.MapFrom(ent => ent.BudgetLimit.Duration));

        CreateMap<Budget, BudgetEntity>()
            .ForPath(ent => ent.BudgetLimit.Amount, opt => opt.MapFrom(model => model.BudgetLimit))
            .ForPath(ent => ent.BudgetLimit.Duration, opt => opt.MapFrom(model => model.DurationInDays));
        
        CreateMap<BudgetEntity, BudgetUpdateResponseDto>()
            .ForPath(ent => ent.BudgetLimit, opt => opt.MapFrom(model => model.BudgetLimit.Amount))
            .ForPath(ent => ent.DurationInDays, opt => opt.MapFrom(model => model.BudgetLimit.Duration));
        
        CreateMap<BudgetEntity, BudgetLookUpVm>()
            .ForPath(ent => ent.BudgetLimit, opt => opt.MapFrom(model => model.BudgetLimit.Amount))
            .ForPath(ent => ent.DurationInDays, opt => opt.MapFrom(model => model.BudgetLimit.Duration));
    }
}