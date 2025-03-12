namespace Infrastructure.DTOs;

public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int StockQuantity { get; set; }
    public DateTime LastStockUpdate { get; set; }
}