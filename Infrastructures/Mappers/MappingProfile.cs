using AutoMapper;
using Infrastructure.DTOs.Category;
using Infrastructure.Models;

namespace Infrastructure.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            // Tambahkan mapping lain sesuai kebutuhan
            // Customer, Inventory, Order, Product, Supplier, User, dll.
        }
    }
} 