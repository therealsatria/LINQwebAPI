using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var products = await _productService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Products retrieved successfully",
                Success = true,
                Products = products
            }
        );
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetAsync(Guid id)
    {
        try
        {
            var product = await _productService.GetAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Product retrieved successfully",
                    Success = true,
                    Product = product
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
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] CreateProductRequest request)
    {
        try
        {
            var product = await _productService.CreateAsync(request);
            return Ok(
                new
                {
                    statusCode = 201,
                    message = "Product created successfully",
                    Success = true,
                    Product = product
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
    public async Task<ActionResult<ProductDto>> UpdateAsync(Guid id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var updatedProduct = await _productService.UpdateAsync(id, request);
            return Ok(
                new
                {
                statusCode = 200,
                message = "Product updated successfully",
                Success = true,
                Product = updatedProduct
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
            await _productService.DeleteAsync(id);
            return Ok(
                new
                {
                    statusCode = 200,
                    message = "Product deleted successfully",
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
