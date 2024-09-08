using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface IExchangeRatesRepository : IGenericRepository<ExchangeRate, ExchangeRate>
{
    Task UpdateRangeAsync(List<ExchangeRate> exchangeRates);
    
    Task<Dictionary<string, ExchangeRate>> GetAllAsDictionaryAsync();

    Task AddRangeAsync(List<ExchangeRate> exchangeRates);
}