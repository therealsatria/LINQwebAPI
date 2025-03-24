using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs.Report;

public class StockHistoryReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalChanges { get; set; }
    public int TotalAdditions { get; set; }
    public int TotalReductions { get; set; }
    public int TotalAdjustments { get; set; }
    public List<StockChangeDto> Changes { get; set; } = new List<StockChangeDto>();
}

public class StockChangeDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public int QuantityChange { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
} 