using Domain.Entities.Transaction;
using Infrastructure.Repositories.interfaces;

namespace Application.utils.CurrencyConverter;

public class MoneyConverter : IConverter<Money>
{
    private readonly IExchangeRatesRepository _exchangeRatesRepository;

    public MoneyConverter(IExchangeRatesRepository exchangeRatesRepository)
    {
        _exchangeRatesRepository = exchangeRatesRepository;
    }

    public async Task<double> ConvertOne(string baseCurrency, double amount, string targetCurrency = "RUB")
    {
        var exchangeRate = await _exchangeRatesRepository.GetByBaseAndTargetCurrency(baseCurrency, targetCurrency);

        if (exchangeRate is null)
        {
            return amount;
        }

        return amount * exchangeRate.Rates;
    }
    
    public async Task<List<Money>> ConvertAll(IEnumerable<Money> money, string targetCurrency = "RUB")
    {
        var exchangeRates = await _exchangeRatesRepository.GetAllAsDictionaryAsync();
        
        return money.Select(m =>
        {
            var key = $"{m.Currency}{targetCurrency}";

            if (exchangeRates.TryGetValue(key, out var rate))
            {
                return Money.Create(m.Amount * rate.Rates, targetCurrency);
            }

            return m;
        }).ToList();
    }
}