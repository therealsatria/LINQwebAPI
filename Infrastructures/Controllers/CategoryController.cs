using Infrastructure.DTOs.Category;
using Infrastructure.Exceptions;
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
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
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
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetAsync(Guid id)
    {
        try
        {
            var category = await _categoryService.GetAsync(id);
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
    public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        try
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
    public async Task<ActionResult<CategoryDto>> UpdateAsync(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        try
        {
            var category = await _categoryService.UpdateAsync(id, request);
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