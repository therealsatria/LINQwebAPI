namespace LINQwebAPI.Infrastructures.DTOs.Order;

public class UpdateOrderRequest
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}