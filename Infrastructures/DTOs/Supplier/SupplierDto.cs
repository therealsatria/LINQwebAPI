namespace Infrastructure.DTOs;

public class SupplierDto
{
    public Guid Id { get; set; }
    public string SupplierName { get; set; } = null!;
    public string ContactPerson { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 