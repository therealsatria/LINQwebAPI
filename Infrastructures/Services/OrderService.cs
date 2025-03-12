using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        }); 
    }   

    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id)
            ?? throw new NotFoundException($"Order with ID {id} not found");
        
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = request.OrderDate,
            TotalAmount = request.TotalAmount
        };

        var createdOrder = await _orderRepository.CreateAsync(order);

        return new OrderDto
        {
            Id = createdOrder.Id,
            CustomerId = createdOrder.CustomerId,
            OrderDate = createdOrder.OrderDate,
            TotalAmount = createdOrder.TotalAmount,
            CreatedAt = createdOrder.CreatedAt,
            UpdatedAt = createdOrder.UpdatedAt
        };      
    }

    public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var order = await _orderRepository.GetAsync(id)
            ?? throw new NotFoundException($"Order with ID {id} not found");

        order.CustomerId = request.CustomerId;
        order.OrderDate = request.OrderDate;
        order.TotalAmount = request.TotalAmount;

        var updatedOrder = await _orderRepository.UpdateAsync(id, order);

        return new OrderDto
        {
            Id = updatedOrder.Id,   
            CustomerId = updatedOrder.CustomerId,
            OrderDate = updatedOrder.OrderDate,
            TotalAmount = updatedOrder.TotalAmount,
            CreatedAt = updatedOrder.CreatedAt,
            UpdatedAt = updatedOrder.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id)
            ?? throw new NotFoundException($"Order with ID {id} not found");
    
        await _orderRepository.DeleteAsync(id);
    }
}
