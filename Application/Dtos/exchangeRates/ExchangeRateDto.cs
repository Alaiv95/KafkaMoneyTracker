namespace Application.Dtos.exchangeRates;

public class ExchangeRateDto
{
    public string BaseCurrency { get; set; }
    
    public string TargetCurrency { get; set; }
    
    public double Rates { get; set; }
}