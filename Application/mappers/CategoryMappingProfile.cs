using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;

namespace Application.mappers;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryEntity, Category>()
            .ForPath(model => model.CategoryName, opt => opt.MapFrom(ent => ent.CategoryValue.CategoryName))
            .ForPath(model => model.CategoryType, opt => opt.MapFrom(ent => ent.CategoryValue.CategoryType));
        
        CreateMap<Category, CategoryEntity>()
            .ForPath(ent => ent.CategoryValue.CategoryName, opt => opt.MapFrom(ent => ent.CategoryName))
            .ForPath(ent => ent.CategoryValue.CategoryType, opt => opt.MapFrom(ent => ent.CategoryType));

        CreateMap<CategoryEntity, CategoryLookupDto>()
            .ForPath(dto => dto.CategoryName, opt => opt.MapFrom(ent => ent.CategoryValue.CategoryName))
            .ForPath(dto => dto.CategoryType, opt => opt.MapFrom(ent => ent.CategoryValue.CategoryType))
            .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id));
    }
}