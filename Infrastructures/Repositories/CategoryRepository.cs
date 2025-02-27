using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return categories ?? new List<Category>();
        }
        public async Task<Category> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }

            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }
            return category;

            // return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.Id = Guid.NewGuid();
            category.CreatedAt = DateTime.UtcNow;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }
        public async Task<Category> UpdateAsync(Guid id, Category category)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            existingCategory.Name = category.Name;

            await _context.SaveChangesAsync();

            return existingCategory;
        }
        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}