using Application.Dtos;
using Application.Dtos.category;
using Application.mediator.interfaces;

namespace Application.handlers.category.command.DeleteCategory;

public class DeleteCategoryCommand : IRequest<CategoryLookupDto>
{
    public Guid CategoryId { get; set; }
    
    public Guid UserId { get; set; }
}