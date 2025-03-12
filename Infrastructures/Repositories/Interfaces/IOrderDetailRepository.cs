using Infrastructure.Models;

namespace Infrastructure.Repositories;
public interface IOrderDetailRepository
{
    Task<IEnumerable<OrderDetail>> GetAllAsync();
    Task<OrderDetail> GetAsync(Guid id);
    Task<OrderDetail> CreateAsync(OrderDetail request);
    Task<OrderDetail> UpdateAsync(Guid id, OrderDetail request);
    Task DeleteAsync(Guid id);
}