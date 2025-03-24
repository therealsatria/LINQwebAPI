using Infrastructure.DTOs.Report;
using Infrastructure.Repositories;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ReportService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<IEnumerable<ProductCategoryReportDto>> GetProductReportByCategoryAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();

        var reports = new List<ProductCategoryReportDto>();

        foreach (var category in categories)
        {
            var categoryProducts = products.Where(p => p.CategoryId == category.Id).ToList();
            
            var report = new ProductCategoryReportDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                ProductCount = categoryProducts.Count,
                TotalPrice = categoryProducts.Sum(p => p.Price),
                AveragePrice = categoryProducts.Any() ? categoryProducts.Average(p => p.Price) : 0,
                Products = categoryProducts.Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedAt = p.CreatedAt
                }).ToList()
            };

            reports.Add(report);
        }

        return reports;
    }

    public async Task<ProductCategoryReportDto> GetProductReportByCategoryIdAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetAsync(categoryId);
        var products = await _productRepository.GetAllAsync();
        
        var categoryProducts = products.Where(p => p.CategoryId == category.Id).ToList();
        
        var report = new ProductCategoryReportDto
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            ProductCount = categoryProducts.Count,
            TotalPrice = categoryProducts.Sum(p => p.Price),
            AveragePrice = categoryProducts.Any() ? categoryProducts.Average(p => p.Price) : 0,
            Products = categoryProducts.Select(p => new ProductSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CreatedAt = p.CreatedAt
            }).ToList()
        };

        return report;
    }
} 