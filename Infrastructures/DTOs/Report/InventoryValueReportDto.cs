using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs.Report;

public class InventoryValueReportDto
{
    public decimal TotalInventoryValue { get; set; }
    public int TotalProducts { get; set; }
    public int TotalStockQuantity { get; set; }
    public DateTime ReportGeneratedAt { get; set; } = DateTime.UtcNow;
    public List<ProductInventoryValueDto> Products { get; set; } = new List<ProductInventoryValueDto>();
}

public class ProductInventoryValueDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime LastStockUpdate { get; set; }
} 