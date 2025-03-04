namespace Infrastructure.DTOs;

public class CreateInventoryRequest
{
    public Guid ProductId { get; set; }
    public int StockQuantity { get; set; }
}