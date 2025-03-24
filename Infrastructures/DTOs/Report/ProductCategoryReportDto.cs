using Infrastructure.Models;

namespace Infrastructure.DTOs.Report;

public class ProductCategoryReportDto
{
    public string CategoryName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public int ProductCount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public List<ProductSummaryDto> Products { get; set; } = new List<ProductSummaryDto>();
}

public class ProductSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
} 