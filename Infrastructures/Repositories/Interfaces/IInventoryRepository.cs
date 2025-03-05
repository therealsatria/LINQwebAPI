using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IInventoryRepository
{
    Task<IEnumerable<Inventory>> GetAllAsync();
    Task<Inventory> GetAsync(Guid id);
    Task<Inventory> CreateAsync(Inventory request);
    Task<Inventory> UpdateAsync(Guid id, Inventory request);
    Task DeleteAsync(Guid id);
}