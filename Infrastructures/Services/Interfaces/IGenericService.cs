using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.DTOs.Generic;

namespace Infrastructure.Services.Interfaces
{
    public interface IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest>
        where TEntity : class
        where TDto : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest request);
        Task<TDto> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TCreateRequest request);
        Task<TDto> UpdateAsync(Guid id, TUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 