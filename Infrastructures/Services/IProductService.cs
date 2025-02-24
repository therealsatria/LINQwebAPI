using Infrastructure.DTOs;

namespace Infrastructure.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto> GetAsync(Guid id);
    Task<ProductDto> CreateAsync(CreateProductRequest request);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request);
    Task DeleteAsync(Guid id);
}