namespace Infrastructure.DTOs;

public class CreateSupplierRequest
{
    public required string SupplierName { get; set; }
    public required string ContactPerson { get; set; }
    public required string ContactPhone { get; set; }
}