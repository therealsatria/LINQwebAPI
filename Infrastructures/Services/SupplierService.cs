using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public SupplierService(ISupplierRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        
        return suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            SupplierName = s.SupplierName,
            ContactPerson = s.ContactPerson,
            ContactPhone = s.ContactPhone,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        });
    }

    public async Task<SupplierDto> GetAsync(Guid id)
    {
        var supplier = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Supplier with ID {id} not found");
        
        return new SupplierDto
        {
            Id = supplier.Id,
            SupplierName = supplier.SupplierName,
            ContactPerson = supplier.ContactPerson,
            ContactPhone = supplier.ContactPhone,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        var supplier = new Supplier
        {
            SupplierName = request.SupplierName,
            ContactPerson = request.ContactPerson,
            ContactPhone = request.ContactPhone,
            CreatedAt = DateTime.UtcNow
        };
        
        var createdSupplier = await _repository.CreateAsync(supplier);
        
        return new SupplierDto
        {
            Id = createdSupplier.Id,
            SupplierName = createdSupplier.SupplierName,
            ContactPerson = createdSupplier.ContactPerson,
            ContactPhone = createdSupplier.ContactPhone,
            CreatedAt = createdSupplier.CreatedAt,
            UpdatedAt = createdSupplier.UpdatedAt
        };
    }

    public async Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var supplier = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Supplier with ID {id} not found");

        supplier.SupplierName = request.SupplierName;
        supplier.ContactPerson = request.ContactPerson;
        supplier.ContactPhone = request.ContactPhone;
        supplier.UpdatedAt = DateTime.UtcNow;

        var updatedSupplier = await _repository.UpdateAsync(id, supplier);
        
        return new SupplierDto
        {
            Id = updatedSupplier.Id,
            SupplierName = updatedSupplier.SupplierName,
            ContactPerson = updatedSupplier.ContactPerson,
            ContactPhone = updatedSupplier.ContactPhone,
            CreatedAt = updatedSupplier.CreatedAt,
            UpdatedAt = updatedSupplier.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var supplier = await _repository.GetAsync(id)
            ?? throw new NotFoundException($"Supplier with ID {id} not found");
            
        await _repository.DeleteAsync(id);
    }
}
