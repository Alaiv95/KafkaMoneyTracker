using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExchangeRatesRepository : GenericRepository<ExchangeRate, ExchangeRate>, IExchangeRatesRepository
{
    public ExchangeRatesRepository(IMoneyTrackerDbContext context, IMapper mapper) 
        : base(context, mapper) { }

    public async Task UpdateRangeAsync(List<ExchangeRate> exchangeRates)
    {
        if (!exchangeRates.Any())
        {
            return;
        }
        
        _dbSet.UpdateRange(exchangeRates);
        await _context.SaveChangesAsync(default);
    }
    
    public async Task AddRangeAsync(List<ExchangeRate> exchangeRates)
    {
        await _dbSet.AddRangeAsync(exchangeRates);
        await _context.SaveChangesAsync(default);
    }

    public async Task<Dictionary<string, ExchangeRate>> GetAllAsDictionaryAsync()
    {
        return await _dbSet.ToDictionaryAsync(x => $"{x.BaseCurrency}{x.TargetCurrency}", x => x);
    }
}