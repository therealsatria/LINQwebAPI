using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs.Report;

public class PurchaseDetailReportDto
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new List<PurchaseItemDto>();
}

public class PurchaseItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
} 