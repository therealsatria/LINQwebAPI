using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> GetAsync(Guid id);
    Task<Customer> CreateAsync(Customer request);
    Task<Customer> UpdateAsync(Guid id, Customer request);
    Task DeleteAsync(Guid id);
}