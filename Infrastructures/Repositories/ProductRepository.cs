using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> GetAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<Product> CreateAsync(Product product)
    {
        // product.Id = Guid.NewGuid();
        // product.CreatedAt = DateTime.UtcNow;
        // _context.Products.Add(product);
        // await _context.SaveChangesAsync();
        // return product;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;

    }
    public async Task<Product> UpdateAsync(Guid id, Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
        
}

