using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllAsync()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Customers retrieved successfully",
                Success = true,
                Customers = customers
            }
        );
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetAsync(Guid id)
    {
        try
        {
            var customer = await _customerService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Customer retrieved successfully",
                    Success = true,
                    Customer = customer
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
    public async Task<ActionResult<CustomerDto>> CreateAsync([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Customer created successfully",
                    Success = true,
                    Customer = customer
                }
            );
        }
        catch (ArgumentException ex)
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
    public async Task<ActionResult<CustomerDto>> UpdateAsync(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            var updatedCustomer = await _customerService.UpdateAsync(id, request);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Customer updated successfully",
                    Success = true,
                    Customer = updatedCustomer
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
            await _customerService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Customer deleted successfully",
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
