using Infrastructure.DTOs;
using Infrastructure.DTOs.Report;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
    }

    [HttpGet("products-by-category")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ProductCategoryReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductReportByCategory()
    {
        try
        {
            var result = await _reportService.GetProductReportByCategoryAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("products-by-category/{categoryId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductCategoryReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductReportByCategoryId(Guid categoryId)
    {
        try
        {
            var result = await _reportService.GetProductReportByCategoryIdAsync(categoryId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("purchase-details")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PurchaseDetailReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPurchaseDetailReport()
    {
        try
        {
            var result = await _reportService.GetPurchaseDetailReportAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("purchase-details/{orderId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PurchaseDetailReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPurchaseDetailReportByOrderId(Guid orderId)
    {
        try
        {
            var result = await _reportService.GetPurchaseDetailReportByOrderIdAsync(orderId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("inventory-value")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(InventoryValueReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInventoryValueReport()
    {
        try
        {
            var result = await _reportService.GetInventoryValueReportAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("stock-history")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StockHistoryReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStockHistoryReport([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _reportService.GetStockHistoryReportAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    [HttpGet("stock-history/product/{productId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StockHistoryReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStockHistoryReportByProductId(Guid productId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _reportService.GetStockHistoryReportByProductIdAsync(productId, startDate, endDate);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}