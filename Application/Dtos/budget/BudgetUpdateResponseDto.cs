namespace Application.Dtos;

public class BudgetUpdateResponseDto
{
    public double BudgetLimit { get; set; }
    
    public int DurationInDays { get; set; }
    
    public Guid Id { get; set; }
}