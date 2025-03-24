using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InventoryHistoryRepository : IInventoryHistoryRepository
{
    private readonly AppDbContext _context;

    public InventoryHistoryRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<InventoryHistory>> GetAllAsync()
    {
        var inventoryHistories = await _context.InventoryHistories
            .AsNoTracking()
            .Include(ih => ih.Product)
            .Include(ih => ih.Inventory)
            .ToListAsync();
            
        return inventoryHistories ?? new List<InventoryHistory>();
    }

    public async Task<IEnumerable<InventoryHistory>> GetByProductIdAsync(Guid productId)
    {
        var inventoryHistories = await _context.InventoryHistories
            .AsNoTracking()
            .Where(ih => ih.ProductId == productId)
            .Include(ih => ih.Product)
            .Include(ih => ih.Inventory)
            .OrderByDescending(ih => ih.ChangedAt)
            .ToListAsync();
            
        return inventoryHistories ?? new List<InventoryHistory>();
    }

    public async Task<IEnumerable<InventoryHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var inventoryHistories = await _context.InventoryHistories
            .AsNoTracking()
            .Where(ih => ih.ChangedAt >= startDate && ih.ChangedAt <= endDate)
            .Include(ih => ih.Product)
            .Include(ih => ih.Inventory)
            .OrderByDescending(ih => ih.ChangedAt)
            .ToListAsync();
            
        return inventoryHistories ?? new List<InventoryHistory>();
    }

    public async Task<InventoryHistory> GetAsync(Guid id)
    {
        var inventoryHistory = await _context.InventoryHistories
            .AsNoTracking()
            .Include(ih => ih.Product)
            .Include(ih => ih.Inventory)
            .FirstOrDefaultAsync(ih => ih.Id == id);
            
        return inventoryHistory;
    }

    public async Task<InventoryHistory> CreateAsync(InventoryHistory inventoryHistory)
    {
        inventoryHistory.Id = Guid.NewGuid();
        _context.InventoryHistories.Add(inventoryHistory);
        await _context.SaveChangesAsync();
        return inventoryHistory;
    }
} 