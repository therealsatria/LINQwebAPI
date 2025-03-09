using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> GetAsync(Guid id);
    Task<Order> CreateAsync(Order request);
    Task<Order> UpdateAsync(Guid id, Order request);
    Task DeleteAsync(Guid id);
}
