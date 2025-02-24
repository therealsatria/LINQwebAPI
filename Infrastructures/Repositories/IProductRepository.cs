using Infrastructure.DTOs;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetAsync(Guid id);
    Task<Product> CreateAsync(Product request);
    Task<Product> UpdateAsync(Guid id, Product request);
    Task DeleteAsync(Guid id);

}