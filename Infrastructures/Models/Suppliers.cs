namespace Infrastructure.Models;

public class Suppliers
{
    public Guid Id { get; set; }
    public required string SupplierName { get; set; }
    public required string ContactPerson { get; set; }
    public required string ContactPhone { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
