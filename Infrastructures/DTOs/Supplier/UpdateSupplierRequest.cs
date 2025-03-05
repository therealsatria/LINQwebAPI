namespace Infrastructure.DTOs;

public class UpdateSupplierRequest
{
    public required string SupplierName { get; set; }
    public required string ContactPerson { get; set; }
    public required string ContactPhone { get; set; }
}