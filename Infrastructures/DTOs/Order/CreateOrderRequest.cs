namespace LINQwebAPI.Infrastructures.DTOs.Order;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}