using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier> GetAsync(Guid id);
    Task<Supplier> CreateAsync(Supplier request);
    Task<Supplier> UpdateAsync(Guid id, Supplier request);
    Task DeleteAsync(Guid id);
}
