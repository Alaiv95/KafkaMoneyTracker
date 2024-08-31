namespace Application.Dtos;

public class TransactionLookupExtendedDto
{
    public double Amount { get; set; }
    
    public bool IsActive { get; set; }

    public CategoryLookupDto Category { get; set; }
}