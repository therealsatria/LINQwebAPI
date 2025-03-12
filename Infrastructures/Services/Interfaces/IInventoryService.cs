using Infrastructure.DTOs;

namespace Infrastructure.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDto>> GetAllAsync();
    Task<InventoryDto> GetAsync(Guid id);
    Task<InventoryDto> CreateAsync(CreateInventoryRequest request);
    Task<InventoryDto> UpdateAsync(Guid id, UpdateInventoryRequest request);
    Task DeleteAsync(Guid id);
}