using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllAsync()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Suppliers retrieved successfully",
                Success = true,
                Suppliers = suppliers
            }
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> GetAsync(Guid id)
    {
        try
        {
            var supplier = await _supplierService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Supplier retrieved successfully",
                    Success = true,
                    Supplier = supplier
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
    public async Task<ActionResult<SupplierDto>> CreateAsync([FromBody] CreateSupplierRequest request)
    {
        try
        {
            var supplier = await _supplierService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Supplier created successfully",
                    Success = true,
                    Supplier = supplier
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
    public async Task<ActionResult<SupplierDto>> UpdateAsync(Guid id, [FromBody] UpdateSupplierRequest request)
    {
        try
        {
            var updatedSupplier = await _supplierService.UpdateAsync(id, request);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Supplier updated successfully",
                    Success = true,
                    Supplier = updatedSupplier
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
            await _supplierService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Supplier deleted successfully",
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
