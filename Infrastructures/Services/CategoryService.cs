using AutoMapper;
using Infrastructure.DTOs.Category;
using Infrastructure.Models;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Services;

public class CategoryService : GenericService<Category, CategoryDto, CreateCategoryRequest, UpdateCategoryRequest>
{
    public CategoryService(IGenericRepository<Category> repository, IMapper mapper) 
        : base(repository, mapper)
    {
        // Tidak perlu implementasi khusus - sudah ditangani oleh GenericService
    }
    
    // Tidak perlu override metode - semua logika CRUD ditangani oleh kelas dasar GenericService
}