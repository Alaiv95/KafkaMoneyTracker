using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
}
