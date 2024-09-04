using Domain.Enums;

namespace Application.Dtos;

public class CategoryLookupDto
{
    public Guid Id { get; set; }

    public CategoryType CategoryType { get; set; }

    public string CategoryName { get; set; }
}