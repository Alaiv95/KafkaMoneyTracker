using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.category.queries;

public class GetCategoriesQuery : IRequest<List<CategoryLookupDto>>
{
    public bool IncludeCustom { get; set; }
}