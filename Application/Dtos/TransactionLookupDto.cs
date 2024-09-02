namespace Application.Dtos;

public class TransactionLookupDto
{
    public double Amount { get; set; }
    
    public string Currency { get; set; }
    
    public bool IsActive { get; set; }

    public Guid BudgetId { get; set; }
}