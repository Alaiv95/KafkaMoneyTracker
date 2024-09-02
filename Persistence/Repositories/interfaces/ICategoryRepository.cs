using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface ICategoryRepository : IGenericRepository<Category, CategoryEntity>
{
    Task<CategoryEntity?> GetByNameAsync(string name);

    Task DeleteAsync(CategoryEntity category);
}