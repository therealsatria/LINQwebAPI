using Infrastructure.DTOs;

namespace Infrastructure.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto> GetAsync(Guid id);
    Task<SupplierDto> CreateAsync(CreateSupplierRequest request);
    Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierRequest request);
    Task DeleteAsync(Guid id);
}

