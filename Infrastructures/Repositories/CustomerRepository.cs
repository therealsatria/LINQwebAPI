using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customers = await _context.Customers.AsNoTracking().ToListAsync();
        return customers ?? new List<Customer>();
    }

    public async Task<Customer> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid customer ID", nameof(id));
        }

        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found");
        }
        return customer;
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        customer.Id = Guid.NewGuid();
        customer.CreatedAt = DateTime.UtcNow;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> UpdateAsync(Guid id, Customer customer)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid customer ID", nameof(id));
        }

        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        var existingCustomer = await _context.Customers.FindAsync(id);
        if (existingCustomer == null)
        {
            throw new KeyNotFoundException("Customer not found");
        }

        existingCustomer.Name = customer.Name;
        existingCustomer.Email = customer.Email;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.Address = customer.Address;
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        _context.Entry(existingCustomer).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return existingCustomer;
    }

    public async Task DeleteAsync(Guid id)      
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid customer ID", nameof(id));
        }

        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found");
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
        
}