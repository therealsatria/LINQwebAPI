using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;
    
    public SupplierRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var suppliers = await _context.Suppliers.AsNoTracking().ToListAsync();
        return suppliers ?? new List<Supplier>();
    }

    public async Task<Supplier> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid supplier ID", nameof(id));
        }

        var supplier = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (supplier == null)
        {
            throw new KeyNotFoundException("Supplier not found");
        }
        return supplier;
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        if (supplier == null)
        {
            throw new ArgumentNullException(nameof(supplier));
        }

        supplier.Id = Guid.NewGuid();
        supplier.CreatedAt = DateTime.UtcNow;

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task<Supplier> UpdateAsync(Guid id, Supplier supplier)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid supplier ID", nameof(id));
        }

        if (supplier == null)
        {
            throw new ArgumentNullException(nameof(supplier));
        }

        var existingSupplier = await _context.Suppliers.FindAsync(id);
        if (existingSupplier == null)
        {
            throw new KeyNotFoundException("Supplier not found");
        }

        existingSupplier.SupplierName = supplier.SupplierName;
        existingSupplier.ContactPerson = supplier.ContactPerson;
        existingSupplier.ContactPhone = supplier.ContactPhone;
        existingSupplier.UpdatedAt = DateTime.UtcNow;

        _context.Entry(existingSupplier).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return existingSupplier;
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid supplier ID", nameof(id));
        }

        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            throw new KeyNotFoundException("Supplier not found");
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}
