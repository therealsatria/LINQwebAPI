using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public InventoryRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync()
    {
        var inventories = await _context.Inventories.AsNoTracking().ToListAsync();
        return inventories ?? new List<Inventory>();
    }

    public async Task<Inventory> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid inventory ID", nameof(id));
        }

        var inventory = await _context.Inventories.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        if (inventory == null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }
        return inventory;
    }

    public async Task<Inventory> CreateAsync(Inventory inventory)
    {
        if (inventory == null)
        {
            throw new ArgumentNullException(nameof(inventory));
        }

        inventory.Id = Guid.NewGuid();
        inventory.CreatedAt = DateTime.UtcNow;

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return inventory;
    }
    public async Task<Inventory> UpdateAsync(Guid id, Inventory inventory)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid inventory ID", nameof(id));
        }

        if (inventory == null)
        {
            throw new ArgumentNullException(nameof(inventory));
        }

        var existingInventory = await _context.Inventories.FindAsync(id);
        if (existingInventory == null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }

        existingInventory.StockQuantity = inventory.StockQuantity;
        existingInventory.LastStockUpdate = DateTime.UtcNow;

        _context.Entry(existingInventory).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return existingInventory;        
    }
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid inventory ID", nameof(id));
        }

        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
    }
}