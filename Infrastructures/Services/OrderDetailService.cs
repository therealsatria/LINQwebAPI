using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    public OrderDetailService(IOrderDetailRepository orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
    }

    public async Task<IEnumerable<OrderDetailDto>> GetAllAsync()
    {
        var orderDetails = await _orderDetailRepository.GetAllAsync();
        return orderDetails.Select(o => new OrderDetailDto
        {
            Id = o.Id,
            OrderId = o.OrderId,
            ProductId = o.ProductId,
            Quantity = o.Quantity,
            UnitPrice = o.UnitPrice,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        }); 
    }   

    public async Task<OrderDetailDto> GetAsync(Guid id)
    {
        var orderDetail = await _orderDetailRepository.GetAsync(id)
            ?? throw new NotFoundException($"OrderDetail with ID {id} not found");
        
        return new OrderDetailDto
        {
            Id = orderDetail.Id,
            OrderId = orderDetail.OrderId,
            ProductId = orderDetail.ProductId,
            Quantity = orderDetail.Quantity,
            UnitPrice = orderDetail.UnitPrice,
            CreatedAt = orderDetail.CreatedAt,
            UpdatedAt = orderDetail.UpdatedAt
        };
    }

    public async Task<OrderDetailDto> CreateAsync(CreateOrderDetailRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var orderDetail = new OrderDetail
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice
        };

        var createdOrderDetail = await _orderDetailRepository.CreateAsync(orderDetail);

        return new OrderDetailDto
        {
            Id = createdOrderDetail.Id,
            OrderId = createdOrderDetail.OrderId,
            ProductId = createdOrderDetail.ProductId,
            Quantity = createdOrderDetail.Quantity,
            UnitPrice = createdOrderDetail.UnitPrice,
            CreatedAt = createdOrderDetail.CreatedAt,
            UpdatedAt = createdOrderDetail.UpdatedAt
        };
    }
    public async Task<OrderDetailDto> UpdateAsync(Guid id, UpdateOrderDetailRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var orderDetail = await _orderDetailRepository.GetAsync(id)
            ?? throw new NotFoundException($"OrderDetail with ID {id} not found");

        orderDetail.OrderId = request.OrderId;
        orderDetail.ProductId = request.ProductId;
        orderDetail.Quantity = request.Quantity;
        orderDetail.UnitPrice = request.UnitPrice;

        var updatedOrderDetail = await _orderDetailRepository.UpdateAsync(id, orderDetail);

        return new OrderDetailDto
        {
            Id = updatedOrderDetail.Id,
            OrderId = updatedOrderDetail.OrderId,
            ProductId = updatedOrderDetail.ProductId,
            Quantity = updatedOrderDetail.Quantity,
            UnitPrice = updatedOrderDetail.UnitPrice,
            CreatedAt = updatedOrderDetail.CreatedAt,
            UpdatedAt = updatedOrderDetail.UpdatedAt
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        var orderDetail = await _orderDetailRepository.GetAsync(id)
            ?? throw new NotFoundException($"OrderDetail with ID {id} not found");
        
        await _orderDetailRepository.DeleteAsync(id);
    }

}