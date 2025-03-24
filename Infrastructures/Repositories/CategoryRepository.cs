using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        // Override metode dari generic repository jika diperlukan
        // atau tambahkan metode khusus untuk Category
    }
}