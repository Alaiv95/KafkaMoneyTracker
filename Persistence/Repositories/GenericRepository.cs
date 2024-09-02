using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Repositories.interfaces;

namespace Infrastructure.Repositories;

public class GenericRepository<TModel, TEntity> : IGenericRepository<TModel, TEntity> 
    where TModel : class
    where TEntity : class
{
    protected readonly IMoneyTrackerDbContext _context;
    protected readonly DbSet<TModel> _dbSet;
    protected readonly IMapper Mapper;

    public GenericRepository(IMoneyTrackerDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
        Mapper = mapper;
    }

    public async Task AddAsync(TEntity entity)
    {
        var model = Mapper.Map<TModel>(entity);
        await _dbSet.AddAsync(model);
        await _context.SaveChangesAsync(default);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var model = await _dbSet.FindAsync(id);
        return Mapper.Map<TEntity>(model);
    }
    
    public async Task<List<TEntity>> GetAllAsync()
    {
        var models = await _dbSet.ToListAsync();

        var res =  Mapper.Map<List<TEntity>>(models);

        return res;
    }

    public async Task<List<TEntity>> SearchAsync(Expression<Func<TModel, bool>> predicate)
    {
        var models = await _dbSet.Where(predicate).ToListAsync();

        var res = Mapper.Map<List<TEntity>>(models);

        return res;
    }
}
