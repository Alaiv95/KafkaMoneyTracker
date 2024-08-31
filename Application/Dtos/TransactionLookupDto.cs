namespace Application.Dtos;

public class TransactionLookupDto
{
    public double Amount { get; set; }
    
    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }
}