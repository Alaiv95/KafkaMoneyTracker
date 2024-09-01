﻿using Application.Dtos;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.commands.UpdateBudget;
using Application.handlers.budget.queries.GetBudgetList;
using AutoMapper;
using Domain.Entities.Budget;
using Infrastructure.Models;

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

        CreateMap<BudgetEntity, Budget>()
            .ForPath(model => model.BudgetLimit, opt => opt.MapFrom(ent => ent.BudgetLimit.Amount))
            .ForPath(model => model.DurationInDays, opt => opt.MapFrom(ent => ent.BudgetLimit.Duration));

        CreateMap<Budget, BudgetEntity>()
            .ForPath(ent => ent.BudgetLimit.Amount, opt => opt.MapFrom(model => model.BudgetLimit))
            .ForPath(ent => ent.BudgetLimit.Duration, opt => opt.MapFrom(model => model.DurationInDays));
    }
}