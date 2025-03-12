using Infrastructure.DTOs.Category;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        // Retrieve all categories from the repository
        var categories = await _repository.GetAllAsync();
        
        // Map category entities to DTOs
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        });
    }
    public async Task<CategoryDto> GetAsync(Guid id)
    {
        // Retrieve the category from the repository
        var category = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Category with ID {id} not found");
        
        // Map category entity to DTO
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        // Validate input parameters
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        // Create a new category entity
        var category = new Category
        {
            Name = request.Name
        };
        
        // Add the category to the repository
        var createdCategory = await _repository.CreateAsync(category);

        // Map the category entity to DTO
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        // Validate input parameters
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        // Retrieve the category from the repository
        var category = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Category with ID {id} not found");
        
        // Update the category entity
        category.Name = request.Name;
        category.UpdatedAt = DateTime.UtcNow;
        
        // Update the category in the repository
        var updatedCategory = await _repository.UpdateAsync(id, category);
        
        // Map the category entity to DTO
        return new CategoryDto
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            CreatedAt = updatedCategory.CreatedAt,
            UpdatedAt = updatedCategory.UpdatedAt
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        // Retrieve the category from the repository
        var category = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Category with ID {id} not found");
        
        // Delete the category from the repository
        await _repository.DeleteAsync(id);
    }
    
    
    
}