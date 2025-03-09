using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        var orders = await _context.Orders.AsNoTracking().ToListAsync();
        return orders ?? new List<Order>();
    }
    public async Task<Order> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID", nameof(id));
        }

        var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            throw new KeyNotFoundException("Order not found");
        }
        return order;
    }   
    public async Task<Order> CreateAsync(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }
        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;   
    }
    public async Task<Order> UpdateAsync(Guid id, Order order)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID", nameof(id));
        }
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }
        var existingOrder = await _context.Orders.FindAsync(id);
        if (existingOrder == null)
        {
            throw new KeyNotFoundException("Order not found");
        }
        existingOrder.CustomerId = order.CustomerId;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.TotalAmount = order.TotalAmount;
        existingOrder.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingOrder).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return existingOrder;   
    }   
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID", nameof(id));
        }
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            throw new KeyNotFoundException("Order not found");
        }   
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();  
    }
}