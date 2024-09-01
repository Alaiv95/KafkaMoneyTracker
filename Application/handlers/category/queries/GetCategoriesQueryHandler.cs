using Application.Dtos;
using Application.mediator.interfaces;
using Infrastructure.Repositories;

namespace Application.handlers.category.queries;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryLookupDto>>
{
    private readonly ICategoryRepository _repository;
    
    public GetCategoriesQueryHandler(ICategoryRepository repository) => _repository = repository;
    
    public async Task<List<CategoryLookupDto>> Handle(GetCategoriesQuery command)
    {
        var categories = await _repository.GetAllAsync();

        if (!command.IncludeCustom)
        {
            categories = categories.Where(c => !c.IsCustom);
        }

        return categories.Select(c => new CategoryLookupDto
        {
            CategoryType = c.CategoryType,
            Id = c.Id,
            CategoryName = c.CategoryName
        }).ToList();
    }
}