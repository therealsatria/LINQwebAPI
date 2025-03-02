namespace Infrastructure.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public required int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastStockUpdate { get; set; } = DateTime.UtcNow;
    public Product? Product { get; set; }
}