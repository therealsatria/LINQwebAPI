namespace Infrastructure.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}