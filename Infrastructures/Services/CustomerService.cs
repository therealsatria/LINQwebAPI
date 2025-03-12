using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt 
        });
    }
    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customer = await _repository.GetAsync(id);
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address, 
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        var createdCustomer = await _repository.CreateAsync(customer);
        return new CustomerDto
        {
            Id = createdCustomer.Id,
            Name = createdCustomer.Name,
            Email = createdCustomer.Email,
            Phone = createdCustomer.Phone,
            Address = createdCustomer.Address,
            CreatedAt = createdCustomer.CreatedAt,
            UpdatedAt = createdCustomer.UpdatedAt
        };
    }
    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var customer = await _repository.GetAsync(id)
        ?? throw new NotFoundException($"Customer with ID {id} not found");

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.UpdatedAt = DateTime.UtcNow;

        var updatedCustomer = await _repository.UpdateAsync(id, customer);
        return new CustomerDto
        {
            Id = updatedCustomer.Id,
            Name = updatedCustomer.Name,
            Email = updatedCustomer.Email,
            Phone = updatedCustomer.Phone,
            Address = updatedCustomer.Address,
            CreatedAt = updatedCustomer.CreatedAt,
            UpdatedAt = updatedCustomer.UpdatedAt
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        // Verify the customer exists before attempting to delete
        var customer = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Customer with ID {id} not found");
            
        // Delete the customer from the repository  
        await _repository.DeleteAsync(id);
    }
}