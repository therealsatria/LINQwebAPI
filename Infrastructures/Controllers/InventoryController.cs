using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetAllAsync()
    {
        var inventories = await _inventoryService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Inventories retrieved successfully",
                Success = true,
                Inventories = inventories
            }
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InventoryDto>> GetAsync(Guid id)
    {
        try
        {
            var inventory = await _inventoryService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Inventory retrieved successfully",
                    Success = true,
                    Inventory = inventory
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
    public async Task<ActionResult<InventoryDto>> CreateAsync([FromBody] CreateInventoryRequest request)
    {
        try
        {
            var inventory = await _inventoryService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Inventory created successfully",
                    Success = true,
                    Inventory = inventory
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
    public async Task<ActionResult<InventoryDto>> UpdateAsync(Guid id, [FromBody] UpdateInventoryRequest request)
    {
        try
        {
            var inventory = await _inventoryService.UpdateAsync(id, request);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Inventory updated successfully",
                    Success = true,
                    Inventory = inventory
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
            await _inventoryService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Inventory deleted successfully",
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
