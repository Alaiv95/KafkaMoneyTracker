using System.Linq.Expressions;

namespace Infrastructure.Repositories.interfaces;

public interface IGenericRepository<TModel, TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task<List<TEntity>> SearchAsync(Expression<Func<TModel, bool>> predicate);
    Task<List<TEntity>> GetAllAsync();
}
