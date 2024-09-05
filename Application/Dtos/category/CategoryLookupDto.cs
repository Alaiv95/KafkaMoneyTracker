using Domain.Enums;

namespace Application.Dtos.category;

public class CategoryLookupDto
{
    public Guid Id { get; set; }

    public CategoryType CategoryType { get; set; }

    public string CategoryName { get; set; }
}