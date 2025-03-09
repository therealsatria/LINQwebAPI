using Infrastructure.DTOs;

namespace Infrastructure.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDto>> GetAllAsync();
        Task<OrderDetailDto> GetAsync(Guid id);
        Task<OrderDetailDto> CreateAsync(CreateOrderDetailRequest request);
        Task<OrderDetailDto> UpdateAsync(Guid id, UpdateOrderDetailRequest request);
        Task DeleteAsync(Guid id);
    }
}