namespace Core.common;

public class BaseCategorySearchFilter
{
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? CategoryId { get; set; }
}
