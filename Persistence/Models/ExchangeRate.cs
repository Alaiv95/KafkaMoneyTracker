namespace Infrastructure.Models;

public class ExchangeRate
{
    public Guid Id { get; set; }
    
    public string BaseCurrency { get; set; }
    
    public string TargetCurrency { get; set; }
    
    public double Rates { get; set; }
    
    public DateTime Date { get; set; }
}