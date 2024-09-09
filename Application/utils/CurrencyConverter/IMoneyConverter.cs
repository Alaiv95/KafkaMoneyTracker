namespace Application.utils.CurrencyConverter;

public interface IConverter<T>
{
    Task<T> ConvertOne(T money, string targetCurrency = "RUB");
    
    Task<List<T>> ConvertAll(IEnumerable<T> money, string? target = "RUB");
}