using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }
    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _repository.GetAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow,
        };
        var createdProduct = await _repository.CreateAsync(product);
        return new ProductDto{
            Id = createdProduct.Id
        };
    }
    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _repository.GetAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        var updatedProduct = await _repository.UpdateAsync(id, product);
        return new ProductDto
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            CreatedAt = updatedProduct.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }
        await _repository.DeleteAsync(id);
    }
}