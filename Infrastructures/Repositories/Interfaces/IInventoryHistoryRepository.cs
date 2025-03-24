using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IInventoryHistoryRepository
{
    Task<IEnumerable<InventoryHistory>> GetAllAsync();
    Task<IEnumerable<InventoryHistory>> GetByProductIdAsync(Guid productId);
    Task<IEnumerable<InventoryHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<InventoryHistory> GetAsync(Guid id);
    Task<InventoryHistory> CreateAsync(InventoryHistory inventoryHistory);
} 