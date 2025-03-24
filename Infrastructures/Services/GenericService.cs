using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Infrastructure.DTOs.Generic;
using Infrastructure.Exceptions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Services.Interfaces;
using AutoMapper;

namespace Infrastructure.Services
{
    public class GenericService<TEntity, TDto, TCreateRequest, TUpdateRequest> : 
        IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest>
        where TEntity : class
        where TDto : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest request)
        {
            var entities = await _repository.GetAllAsync();
            var query = entities.AsQueryable();

            // Apply search if specified
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var properties = typeof(TEntity).GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.Name);

                if (properties.Any())
                {
                    foreach (var prop in properties)
                    {
                        query = query.Where(DynamicExpressionParser.ParseLambda<TEntity, bool>(
                            null,
                            false,
                            $"{prop}.Contains(@0)",
                            request.SearchTerm
                        ));
                    }
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var property = typeof(TEntity).GetProperty(request.SortBy, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (property != null)
                {
                    var sortExpression = request.SortDesc ? $"{request.SortBy} DESC" : request.SortBy;
                    query = DynamicQueryableExtensions.OrderBy(query, sortExpression);
                }
            }

            // Calculate total count
            var totalCount = query.Count();

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to DTOs
            var dtos = _mapper.Map<List<TDto>>(items);

            return PagedResponse<TDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> CreateAsync(TCreateRequest request)
        {
            // Map request to entity
            var entity = _mapper.Map<TEntity>(request);
            
            // Create entity
            var createdEntity = await _repository.CreateAsync(entity);
            
            // Map entity to DTO
            return _mapper.Map<TDto>(createdEntity);
        }

        public virtual async Task<TDto> UpdateAsync(Guid id, TUpdateRequest request)
        {
            // Check if entity exists
            if (!await _repository.ExistsAsync(id))
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

            // Map request to entity
            var entity = _mapper.Map<TEntity>(request);
            
            // Update entity
            var updatedEntity = await _repository.UpdateAsync(id, entity);
            if (updatedEntity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");
            
            // Map entity to DTO
            return _mapper.Map<TDto>(updatedEntity);
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found");

            return await _repository.DeleteAsync(id);
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }
    }
} 