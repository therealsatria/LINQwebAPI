using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var product = await _productService.GetAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest request)
    {
        var createdProduct = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetAsync), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductRequest request)
    {
        var updatedProduct = await _productService.UpdateAsync(id, request);
        return Ok(updatedProduct);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}