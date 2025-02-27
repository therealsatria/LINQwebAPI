using Infrastructure.DTOs.Category;

namespace Infrastructure.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetAsync(Guid id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
        Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryRequest request);
        Task DeleteAsync(Guid id);
    }
}