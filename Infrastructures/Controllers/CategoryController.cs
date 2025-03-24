using Infrastructure.DTOs.Category;
using Infrastructure.Models;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : GenericController<Category, CategoryDto, CreateCategoryRequest, UpdateCategoryRequest>
    {
        private readonly IGenericService<Category, CategoryDto, CreateCategoryRequest, UpdateCategoryRequest> _genericService;

        public CategoryController(IGenericService<Category, CategoryDto, CreateCategoryRequest, UpdateCategoryRequest> service)
            : base(service)
        {
            _genericService = service;
        }

        // Override metode dari generic controller jika diperlukan
        // atau tambahkan metode khusus untuk Category
        
        // Contoh: Override untuk menambahkan kebijakan otorisasi khusus
        [HttpPost]
        // [Authorize(Roles = "Admin")] // Hanya admin yang bisa membuat kategori
        public override async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            return await base.Create(request);
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")] // Hanya admin yang bisa mengubah kategori
        public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            return await base.Update(id, request);
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")] // Hanya admin yang bisa menghapus kategori
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }

        // Endpoint Get tidak dioverride karena bisa diakses secara publik (AllowAnonymous sudah diatur di kelas dasar)
    }
}