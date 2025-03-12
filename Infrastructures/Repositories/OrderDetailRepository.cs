using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderDetailRepository: IOrderDetailRepository
{
    private readonly AppDbContext _context;
    public OrderDetailRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<OrderDetail>> GetAllAsync()
    {
        var orderDetails = await _context.OrderDetails.AsNoTracking().ToListAsync();
        return orderDetails ?? new List<OrderDetail>();
    }
    public async Task<OrderDetail> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order detail ID", nameof(id));
        }

        var orderDetail = await _context.OrderDetails.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        if (orderDetail == null)
        {
            throw new KeyNotFoundException("Order detail not found");
        }
        return orderDetail;
    }   
    public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
    {
        if (orderDetail == null)
        {
            throw new ArgumentNullException(nameof(orderDetail));
        }
        orderDetail.Id = Guid.NewGuid();
        orderDetail.CreatedAt = DateTime.UtcNow;
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return orderDetail;   
    }
    public async Task<OrderDetail> UpdateAsync(Guid id, OrderDetail orderDetail)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order detail ID", nameof(id));
        }
        if (orderDetail == null)
        {
            throw new ArgumentNullException(nameof(orderDetail));
        }
        var existingOrderDetail = await _context.OrderDetails.FindAsync(id);
        if (existingOrderDetail == null)
        {
            throw new KeyNotFoundException("Order detail not found");
        }
        existingOrderDetail.OrderId = orderDetail.OrderId;
        existingOrderDetail.ProductId = orderDetail.ProductId;
        existingOrderDetail.Quantity = orderDetail.Quantity;
        existingOrderDetail.UnitPrice = orderDetail.UnitPrice;
        existingOrderDetail.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existingOrderDetail;
    }
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid order detail ID", nameof(id));
        }
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null)
        {
            throw new KeyNotFoundException("Order detail not found");
        }
        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
    }
}