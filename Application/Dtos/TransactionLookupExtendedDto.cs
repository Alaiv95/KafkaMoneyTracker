namespace Application.Dtos;

public class TransactionLookupExtendedDto
{
    public double Amount { get; set; }
    
    public bool IsActive { get; set; }
    
    public string Currency { get; set; }

    // public CategoryLookupDto Budget { get; set; }
}