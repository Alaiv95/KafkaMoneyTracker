using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);

    Task DeleteAsync(Category category);
}