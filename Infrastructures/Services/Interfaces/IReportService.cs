using Infrastructure.DTOs.Report;

namespace Infrastructure.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ProductCategoryReportDto>> GetProductReportByCategoryAsync();
    Task<ProductCategoryReportDto> GetProductReportByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<PurchaseDetailReportDto>> GetPurchaseDetailReportAsync();
    Task<PurchaseDetailReportDto> GetPurchaseDetailReportByOrderIdAsync(Guid orderId);
    Task<InventoryValueReportDto> GetInventoryValueReportAsync();
    Task<StockHistoryReportDto> GetStockHistoryReportAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<StockHistoryReportDto> GetStockHistoryReportByProductIdAsync(Guid productId, DateTime? startDate = null, DateTime? endDate = null);
} 