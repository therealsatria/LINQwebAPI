using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetAsync(Guid id);
    Task<Category> CreateAsync(Category request);
    Task<Category> UpdateAsync(Guid id, Category request);
    Task DeleteAsync(Guid id);

}