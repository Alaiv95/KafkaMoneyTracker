namespace Infrastructure.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
}
