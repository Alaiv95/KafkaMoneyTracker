using Domain.Enums;

namespace Domain.Entities;

public class CategoryEntity
{
    private CategoryEntity() {}
    
    public Guid Id { get; private set; }

    public CategoryValue CategoryValue { get; private set; }
    
    public bool IsCustom { get; private set; }
    
    public Guid? CreatedBy { get; private set; }

    public static CategoryEntity Create(CategoryValue categoryValue, Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId can't be empty guid");
        }

        return new()
        {
            CategoryValue = categoryValue,
            CreatedBy = userId,
            IsCustom = true,
            Id = Guid.NewGuid(),
        };
    }
}

public record CategoryValue
{
    private const int MinLength = 3;
    private CategoryValue() { }
    
    public CategoryType CategoryType { get; private set; }
    
    public String CategoryName { get; private set; }

    public static CategoryValue Create(CategoryType type, String name)
    {
        if (string.IsNullOrEmpty(name) || name.Length < MinLength)
        {
            throw new ArgumentException("duration can't equals or be less then 0");
        }

        return new()
        {   
            CategoryType = type,
            CategoryName = name
        };
    }
}