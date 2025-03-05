namespace Infrastructure.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid CategoryId { get; set; }
    public Guid SupplierId { get; set; }
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
    public ICollection<Inventory>? Inventories { get; set; } = new List<Inventory>();
}