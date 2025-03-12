using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService ?? throw new ArgumentNullException(nameof(orderDetailService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetAllAsync()
    {
        var orderDetails = await _orderDetailService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Order details retrieved successfully",
                Success = true,
                OrderDetails = orderDetails
            }
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDetailDto>> GetAsync(Guid id)
    {
        try
        {
            var orderDetail = await _orderDetailService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order detail retrieved successfully",
                    Success = true,
                    OrderDetail = orderDetail
                }
            );
        }
        catch (NotFoundException ex)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = ex.Message,
                    Success = false
                }
            );
        }
    }
    [HttpPost]
    public async Task<ActionResult<OrderDetailDto>> CreateAsync([FromBody] CreateOrderDetailRequest request)
    {
        try
        {
            var orderDetail = await _orderDetailService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Order detail created successfully",
                    Success = true,
                    OrderDetail = orderDetail
                }
            );
        }
        catch (NotFoundException ex)
        {
            return BadRequest(
                new
                {
                    statusCode = 404,
                    message = ex.Message,
                    Success = false
                }
            );
        }
    }
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OrderDetailDto>> UpdateAsync(Guid id, [FromBody] UpdateOrderDetailRequest request)
    {
        try
        {
            var orderDetail = await _orderDetailService.UpdateAsync(id, request);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order detail updated successfully",
                    Success = true,
                    OrderDetail = orderDetail
                }
            );
        }
        catch (NotFoundException ex)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = ex.Message,
                    Success = false
                }
            );
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(
                new
                {
                    statusCode = 400,
                    message = ex.Message,
                    Success = false
                }
            );
        }
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<OrderDetailDto>> DeleteAsync(Guid id)
    {
        try
        {
            await _orderDetailService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order detail deleted successfully",
                    Success = true,
                }
            );
        }
        catch (NotFoundException ex)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = ex.Message,
                    Success = false
                }
            );
        }
    }
}
