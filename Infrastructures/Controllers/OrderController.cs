using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Orders retrieved successfully",  
                Success = true,
                Orders = orders
            }
        );
    }   

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetAsync(Guid id)
    {
        try
        {
            var order = await _orderService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order retrieved successfully",
                    Success = true,
                    Order = order
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
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = await _orderService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Order created successfully",
                    Success = true,
                    Order = order
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

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OrderDto>> UpdateAsync(Guid id, [FromBody] UpdateOrderRequest request)
    {
        try
        {
            var order = await _orderService.UpdateAsync(id, request);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order updated successfully",
                    Success = true,
                    Order = order
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
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            await _orderService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Order deleted successfully",
                    Success = true
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