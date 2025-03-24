using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.DTOs.Generic;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class GenericController<TEntity, TDto, TCreateRequest, TUpdateRequest> : ControllerBase
        where TEntity : class
        where TDto : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        protected readonly IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest> _service;

        protected GenericController(IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                var response = ApiResponse<IEnumerable<TDto>>.SuccessResponse(
                    items, 
                    $"{typeof(TEntity).Name} list retrieved successfully"
                );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<IEnumerable<TDto>>.ErrorResponse($"Error retrieving {typeof(TEntity).Name} list: {ex.Message}"));
            }
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            try
            {
                var pagedItems = await _service.GetPagedAsync(request);
                var response = ApiResponse<PagedResponse<TDto>>.SuccessResponse(
                    pagedItems, 
                    $"{typeof(TEntity).Name} list retrieved successfully"
                );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<PagedResponse<TDto>>.ErrorResponse($"Error retrieving {typeof(TEntity).Name} list: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                var response = ApiResponse<TDto>.SuccessResponse(
                    item, 
                    $"{typeof(TEntity).Name} retrieved successfully"
                );
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<TDto>.ErrorResponse($"Error retrieving {typeof(TEntity).Name}: {ex.Message}"));
            }
        }

        [HttpPost]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Create([FromBody] TCreateRequest request)
        {
            try
            {
                var createdItem = await _service.CreateAsync(request);
                var response = ApiResponse<TDto>.SuccessResponse(
                    createdItem, 
                    $"{typeof(TEntity).Name} created successfully"
                );
                
                // Get the ID property of the DTO
                var idProperty = typeof(TDto).GetProperty("Id");
                if (idProperty != null)
                {
                    var id = idProperty.GetValue(createdItem);
                    return CreatedAtAction(nameof(GetById), new { id }, response);
                }
                
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<TDto>.ErrorResponse($"Error creating {typeof(TEntity).Name}: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdateRequest request)
        {
            try
            {
                var updatedItem = await _service.UpdateAsync(id, request);
                var response = ApiResponse<TDto>.SuccessResponse(
                    updatedItem, 
                    $"{typeof(TEntity).Name} updated successfully"
                );
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<TDto>.ErrorResponse($"Error updating {typeof(TEntity).Name}: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                var response = ApiResponse<bool>.SuccessResponse(
                    result, 
                    $"{typeof(TEntity).Name} deleted successfully"
                );
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<bool>.ErrorResponse($"Error deleting {typeof(TEntity).Name}: {ex.Message}"));
            }
        }
    }
} 