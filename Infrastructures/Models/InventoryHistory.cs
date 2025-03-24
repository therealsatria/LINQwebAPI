namespace Infrastructure.Models;

public class InventoryHistory
{
    public Guid Id { get; set; }
    public Guid InventoryId { get; set; }
    public Guid ProductId { get; set; }
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public int QuantityChange { get; set; }
    public string ChangeType { get; set; } = string.Empty; // "Addition", "Reduction", "Adjustment"
    public string Notes { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Inventory? Inventory { get; set; }
    public Product? Product { get; set; }
} 