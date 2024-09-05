namespace Application.Dtos;

public class BudgetLookUpDto
{
    public double BudgetLimit { get; set; }

    public int DurationInDays { get; set; }

    public string CategoryName { get; set; }
}