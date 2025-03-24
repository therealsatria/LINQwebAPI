using Infrastructure.DTOs.Report;

namespace Infrastructure.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ProductCategoryReportDto>> GetProductReportByCategoryAsync();
    Task<ProductCategoryReportDto> GetProductReportByCategoryIdAsync(Guid categoryId);
} 