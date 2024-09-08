using System.Globalization;
using Application.Dtos.exchangeRates;
using Core.common;
using Infrastructure.HttpClientS;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Jobs;

public class UpdateExchangeRatesJob : BackgroundService
{
    private readonly IAppHttpClient _httpClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer? _timer;

    public UpdateExchangeRatesJob(IAppHttpClient httpClient, IServiceScopeFactory scopeFactory)
    {
        _httpClient = httpClient;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(StartJob, null, TimeSpan.Zero, TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private async void StartJob(object? _)
    {
        var result = await FetchExchangeRatesDataAsync();
        await UpdateExchangeRatesAsync(result);
    }
    
    private async Task<List<ExchangeRateDto>> FetchExchangeRatesDataAsync()
    {
        var names = Enum.GetNames(typeof(CurrencyTypes));
        var exchangeRates = new List<ExchangeRateDto>();

        foreach (var name in names)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "base_currency", name },
                { "currencies", string.Join(",", names.Where(n => n != name)) }
            };

            var ratesResponse = await _httpClient.GetAsJsonAsync<ExchangeRatesResponse>(queryParams);

            AppendFetchedRates(ratesResponse?.Data, name, exchangeRates);
        }

        return exchangeRates;
    }

    private void AppendFetchedRates(Dictionary<string, double>? rates, string name, List<ExchangeRateDto> result)
    {
        if (rates is null)
        {
            return;
        }

        foreach (var data in rates)
        {
            var newRate = new ExchangeRateDto
            {
                BaseCurrency = name,
                TargetCurrency = data.Key,
                Rates = data.Value
            };

            result.Add(newRate);
        }
    }

    private async Task UpdateExchangeRatesAsync(List<ExchangeRateDto> result)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IExchangeRatesRepository>();

        var existingRates = await repo.GetAllAsDictionaryAsync();
        var updatedExchangeRates = new List<ExchangeRate>();
        var newExchangeRates = new List<ExchangeRate>();

        foreach (var dto in result)
        {
            AddUpdatedRates(dto, existingRates, updatedExchangeRates);
            AddNewRates(dto, existingRates, newExchangeRates);
        }

        await repo.UpdateRangeAsync(updatedExchangeRates);
        await repo.AddRangeAsync(newExchangeRates);
    }

    private void AddUpdatedRates(ExchangeRateDto dto, Dictionary<string, ExchangeRate> rates,
        List<ExchangeRate> updatedRates)
    {
        var dtoCurrency = $"{dto.BaseCurrency}{dto.TargetCurrency}";
        var isExist = rates.TryGetValue(dtoCurrency, out var rate);

        if (!isExist)
        {
            return;
        }

        var prevRates = rate!.Rates.ToString(CultureInfo.InvariantCulture);
        var newRates = dto.Rates.ToString(CultureInfo.InvariantCulture);

        if (prevRates != newRates)
        {
            rate.Rates = dto.Rates;
            updatedRates.Add(rate);
        }
    }

    private void AddNewRates(ExchangeRateDto dto, Dictionary<string, ExchangeRate> rates,
        List<ExchangeRate> newRates)
    {
        var dtoCurrency = $"{dto.BaseCurrency}{dto.TargetCurrency}";
        var isExist = rates.ContainsKey(dtoCurrency);

        if (isExist)
        {
            return;
        }
        
        newRates.Add(
            new()
            {
                Id = new Guid(),
                TargetCurrency = dto.TargetCurrency,
                BaseCurrency = dto.BaseCurrency,
                Rates = dto.Rates,
                Date = DateTime.Now
            }
        );
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}