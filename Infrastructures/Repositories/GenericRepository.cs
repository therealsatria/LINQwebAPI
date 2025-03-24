using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            // Mencari properti Id pada entity
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have an Id property");

            // Mencari entity berdasarkan Id
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id").Equals(id));
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            // Set Id jika property Id ada dan null
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                // Cek apakah Id sudah di-set
                var idValue = (Guid)idProperty.GetValue(entity);
                if (idValue == Guid.Empty)
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }
            }

            // Set CreatedAt jika ada
            var createdAtProperty = typeof(T).GetProperty("CreatedAt");
            if (createdAtProperty != null && createdAtProperty.PropertyType == typeof(DateTime))
            {
                createdAtProperty.SetValue(entity, DateTime.UtcNow);
            }

            // Set UpdatedAt jika ada
            var updatedAtProperty = typeof(T).GetProperty("UpdatedAt");
            if (updatedAtProperty != null && updatedAtProperty.PropertyType == typeof(DateTime))
            {
                updatedAtProperty.SetValue(entity, DateTime.UtcNow);
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(Guid id, T entity)
        {
            // Mencari entity yang akan diupdate
            var existingEntity = await GetByIdAsync(id);
            if (existingEntity == null)
                return null;

            // Set Id pada entity yang akan diupdate
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                idProperty.SetValue(entity, id);
            }

            // Set UpdatedAt jika ada
            var updatedAtProperty = typeof(T).GetProperty("UpdatedAt");
            if (updatedAtProperty != null && updatedAtProperty.PropertyType == typeof(DateTime))
            {
                updatedAtProperty.SetValue(entity, DateTime.UtcNow);
            }

            // Pertahankan CreatedAt jika ada
            var createdAtProperty = typeof(T).GetProperty("CreatedAt");
            if (createdAtProperty != null && createdAtProperty.PropertyType == typeof(DateTime))
            {
                var existingCreatedAt = createdAtProperty.GetValue(existingEntity);
                createdAtProperty.SetValue(entity, existingCreatedAt);
            }

            _context.Entry(existingEntity).State = EntityState.Detached;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<Guid>(e, "Id").Equals(id));
        }
    }
} 