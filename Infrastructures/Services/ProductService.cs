using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    public ProductService(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        // Retrieve all products from the repository
        var products = await _repository.GetAllAsync();
        
        // Map product entities to DTOs
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            CategoryId = p.CategoryId,
            SupplierId = p.SupplierId
        });
    }
    public async Task<ProductDto> GetAsync(Guid id)
    {
        // Retrieve the product from the repository
        var product = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Product with ID {id} not found");
        
        // Map product entity to DTO
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId
        };
    }
    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        // Validate input parameters
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        // Create a new product entity
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow,
            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId
        };
        
        // Persist the new product to the database
        var createdProduct = await _repository.CreateAsync(product);
        
        // Map the created product to DTO and return
        return new ProductDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
            CreatedAt = createdProduct.CreatedAt,
            UpdatedAt = createdProduct.UpdatedAt,
            CategoryId = createdProduct.CategoryId,
            SupplierId = createdProduct.SupplierId
        };
    }
    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var product = await _repository.GetAsync(id) 
        ?? throw new NotFoundException($"Product with ID {id} not found");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.UpdatedAt = DateTime.UtcNow;
        product.CategoryId = request.CategoryId;
        product.SupplierId = request.SupplierId;

        var updatedProduct = await _repository.UpdateAsync(id, product);
        return new ProductDto
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            CreatedAt = updatedProduct.CreatedAt,
            UpdatedAt = updatedProduct.UpdatedAt,
            CategoryId = updatedProduct.CategoryId,
            SupplierId = updatedProduct.SupplierId
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        // Verify the product exists before attempting to delete
        var product = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Product with ID {id} not found");
        
        // Delete the product from the repository
        await _repository.DeleteAsync(id);
    }
}