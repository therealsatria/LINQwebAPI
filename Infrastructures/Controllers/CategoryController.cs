using Infrastructure.DTOs.Category;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(CategoryService));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(
            new
            {
                statusCode = 200,
                message = "Categories retrieved successfully",
                Success = true,
                Categories = categories
            }
        );
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var category = await _categoryService.GetAsync(id);
        if (category == null)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = "Category not found",
                    Success = false
                }
            );
        }
        return Ok(
            new
            {
                statusCode = 200,
                message = "Category retrieved successfully",
                Success = true,
                Category = category
            }
        );
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        return Ok(
            new
            {
                statusCode = 201,
                message = "Category created successfully",
                Success = true,
                Category = category
            }
        );
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var category = await _categoryService.UpdateAsync(id, request);
        if (category == null)
        {
            return NotFound(
                new
                {
                    statusCode = 404,
                    message = "Category not found",
                    Success = false
                }
            );
        }
        return Ok(
            new
            {
                statusCode = 200,
                message = "Category updated successfully",
                Success = true,
                Category = category
            }
        );
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok(
            new
            {
                statusCode = 200,
                message = "Category deleted successfully",
                Success = true
            }
        );
    }
}