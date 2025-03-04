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
        var product = await _productService.GetAsync(id);
        if (product == null)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = "Product not found",
                    Success = false
                }
            );
        }
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
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] CreateProductRequest request)
    {
        var Product = await _productService.CreateAsync(request);
        return Ok(
            new
            {
                statusCode = 201,
                message = "Product created successfully",
                Success = true,
                Product
            }
        );
    }
        
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> UpdateAsync(Guid id, [FromBody] UpdateProductRequest request)
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
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
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
}