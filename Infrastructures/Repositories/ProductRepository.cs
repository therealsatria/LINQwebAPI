using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }
    public async Task<Product> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID", nameof(id));
        }

        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Product> CreateAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }
    public async Task<Product> UpdateAsync(Guid id, Product product)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID", nameof(id));
        }

        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException("Product not found");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        existingProduct.CategoryId = product.CategoryId;

        _context.Entry(existingProduct).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return existingProduct;
    }
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID", nameof(id));
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
        
}

