using Infrastructure.DTOs;

namespace Infrastructure.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto> GetAsync(Guid id);
    Task<OrderDto> CreateAsync(CreateOrderRequest request);
    Task<OrderDto> UpdateAsync(Guid id, UpdateOrderRequest request);
    Task DeleteAsync(Guid id);
}