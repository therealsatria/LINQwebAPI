using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
    }

    public async Task<IEnumerable<InventoryDto>> GetAllAsync()
    {
        var inventories = await _inventoryRepository.GetAllAsync();
        return inventories.Select(i => new InventoryDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            StockQuantity = i.StockQuantity,
            LastStockUpdate = i.LastStockUpdate
        });
    }

    public async Task<InventoryDto> GetAsync(Guid id)
    {
        var inventory = await _inventoryRepository.GetAsync(id)
            ?? throw new NotFoundException($"Inventory with ID {id} not found");
        
        return new InventoryDto
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            StockQuantity = inventory.StockQuantity,
            LastStockUpdate = inventory.LastStockUpdate
        };
    }

    public async Task<InventoryDto> CreateAsync(CreateInventoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        var inventory = new Inventory
        {
            ProductId = request.ProductId,
            StockQuantity = request.StockQuantity,
            LastStockUpdate = DateTime.UtcNow
        };
        
        var createdInventory = await _inventoryRepository.CreateAsync(inventory);
        
        return new InventoryDto
        {
            Id = createdInventory.Id,
            ProductId = createdInventory.ProductId,
            StockQuantity = createdInventory.StockQuantity,
            LastStockUpdate = createdInventory.LastStockUpdate
        };
    }

    public async Task<InventoryDto> UpdateAsync(Guid id, UpdateInventoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var inventory = await _inventoryRepository.GetAsync(id)
            ?? throw new NotFoundException($"Inventory with ID {id} not found");

        inventory.StockQuantity = request.StockQuantity;
        inventory.LastStockUpdate = DateTime.UtcNow;

        var updatedInventory = await _inventoryRepository.UpdateAsync(id, inventory);
        return new InventoryDto
        {
            Id = updatedInventory.Id,
            ProductId = updatedInventory.ProductId,
            StockQuantity = updatedInventory.StockQuantity,
            LastStockUpdate = updatedInventory.LastStockUpdate
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var inventory = await _inventoryRepository.GetAsync(id)
            ?? throw new NotFoundException($"Inventory with ID {id} not found");
        
        await _inventoryRepository.DeleteAsync(id);
    }
}