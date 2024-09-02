namespace Domain.Entities.Budget;

public record BudgetInfo
{
    public double BudgetLimit { get; set; }
    
    public int DurationInDays { get; set; }
    
    public string CategoryName { get; set; }
}