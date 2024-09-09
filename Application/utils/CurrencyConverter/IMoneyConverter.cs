namespace Application.utils.CurrencyConverter;

public interface IConverter<T>
{
    Task<double> ConvertOne(string baseCurrency, double amount, string targetCurrency = "RUB");
    
    Task<List<T>> ConvertAll(IEnumerable<T> money, string? target = "RUB");
}