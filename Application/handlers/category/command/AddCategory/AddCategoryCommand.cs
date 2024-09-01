using Application.Dtos;
using Application.mediator.interfaces;

namespace Application.handlers.category.command.AddCategory;

public class AddCategoryCommand : IRequest<CategoryLookupDto>
{
    public string Name { get; set; }
    
    public Guid UserId { get; set; }
}