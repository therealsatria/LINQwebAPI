using Infrastructure.DTOs.Report;
using Infrastructure.Repositories;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryHistoryRepository _inventoryHistoryRepository;

    public ReportService(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository,
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        ICustomerRepository customerRepository,
        IInventoryRepository inventoryRepository,
        IInventoryHistoryRepository inventoryHistoryRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _inventoryHistoryRepository = inventoryHistoryRepository ?? throw new ArgumentNullException(nameof(inventoryHistoryRepository));
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

    public async Task<IEnumerable<PurchaseDetailReportDto>> GetPurchaseDetailReportAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        var orderDetails = await _orderDetailRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var reports = new List<PurchaseDetailReportDto>();

        foreach (var order in orders)
        {
            var orderItems = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

            if (customer == null || !orderItems.Any())
                continue;

            var report = new PurchaseDetailReportDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                CustomerName = customer.Name,
                CustomerEmail = customer.Email,
                TotalAmount = order.TotalAmount,
                TotalItems = orderItems.Sum(oi => oi.Quantity),
                Items = orderItems.Select(oi => {
                    var product = products.FirstOrDefault(p => p.Id == oi.ProductId);
                    var category = product != null 
                        ? categories.FirstOrDefault(c => c.Id == product.CategoryId) 
                        : null;
                    
                    return new PurchaseItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = product?.Name ?? "Product Not Found",
                        ProductDescription = product?.Description ?? "",
                        CategoryName = category?.Name ?? "Category Not Found",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Subtotal = oi.Quantity * oi.UnitPrice
                    };
                }).ToList()
            };

            reports.Add(report);
        }

        return reports;
    }

    public async Task<PurchaseDetailReportDto> GetPurchaseDetailReportByOrderIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found");

        var orderDetails = await _orderDetailRepository.GetAllAsync();
        var orderItems = orderDetails.Where(od => od.OrderId == order.Id).ToList();
        
        if (!orderItems.Any())
            throw new KeyNotFoundException($"Order items for order ID {orderId} not found");

        var products = await _productRepository.GetAllAsync();
        var customer = await _customerRepository.GetAsync(order.CustomerId);
        var categories = await _categoryRepository.GetAllAsync();

        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {order.CustomerId} not found");

        var report = new PurchaseDetailReportDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            CustomerName = customer.Name,
            CustomerEmail = customer.Email,
            TotalAmount = order.TotalAmount,
            TotalItems = orderItems.Sum(oi => oi.Quantity),
            Items = orderItems.Select(oi => {
                var product = products.FirstOrDefault(p => p.Id == oi.ProductId);
                var category = product != null 
                    ? categories.FirstOrDefault(c => c.Id == product.CategoryId) 
                    : null;
                
                return new PurchaseItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = product?.Name ?? "Product Not Found",
                    ProductDescription = product?.Description ?? "",
                    CategoryName = category?.Name ?? "Category Not Found",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Quantity * oi.UnitPrice
                };
            }).ToList()
        };

        return report;
    }

    public async Task<InventoryValueReportDto> GetInventoryValueReportAsync()
    {
        var inventories = await _inventoryRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var productInventories = new List<ProductInventoryValueDto>();
        decimal totalValue = 0;
        int totalQuantity = 0;

        foreach (var inventory in inventories)
        {
            var product = products.FirstOrDefault(p => p.Id == inventory.ProductId);
            if (product == null) continue;

            var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
            decimal productValue = product.Price * inventory.StockQuantity;
            totalValue += productValue;
            totalQuantity += inventory.StockQuantity;

            productInventories.Add(new ProductInventoryValueDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                CategoryName = category?.Name ?? "Category Not Found",
                StockQuantity = inventory.StockQuantity,
                UnitPrice = product.Price,
                TotalValue = productValue,
                LastStockUpdate = inventory.LastStockUpdate
            });
        }

        return new InventoryValueReportDto
        {
            TotalInventoryValue = totalValue,
            TotalProducts = productInventories.Count,
            TotalStockQuantity = totalQuantity,
            ReportGeneratedAt = DateTime.UtcNow,
            Products = productInventories.OrderByDescending(p => p.TotalValue).ToList()
        };
    }

    public async Task<StockHistoryReportDto> GetStockHistoryReportAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        DateTime effectiveStartDate = startDate ?? DateTime.UtcNow.AddMonths(-1);
        DateTime effectiveEndDate = endDate ?? DateTime.UtcNow;
        
        var histories = await _inventoryHistoryRepository.GetByDateRangeAsync(effectiveStartDate, effectiveEndDate);
        var products = await _productRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var changes = new List<StockChangeDto>();

        foreach (var history in histories)
        {
            var product = products.FirstOrDefault(p => p.Id == history.ProductId);
            if (product == null) continue;

            var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);

            changes.Add(new StockChangeDto
            {
                Id = history.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                CategoryName = category?.Name ?? "Category Not Found",
                PreviousQuantity = history.PreviousQuantity,
                NewQuantity = history.NewQuantity,
                QuantityChange = history.QuantityChange,
                ChangeType = history.ChangeType,
                Notes = history.Notes,
                ChangedAt = history.ChangedAt
            });
        }

        return new StockHistoryReportDto
        {
            StartDate = effectiveStartDate,
            EndDate = effectiveEndDate,
            TotalChanges = changes.Count,
            TotalAdditions = changes.Count(c => c.ChangeType == "Addition"),
            TotalReductions = changes.Count(c => c.ChangeType == "Reduction"),
            TotalAdjustments = changes.Count(c => c.ChangeType == "Adjustment"),
            Changes = changes.OrderByDescending(c => c.ChangedAt).ToList()
        };
    }

    public async Task<StockHistoryReportDto> GetStockHistoryReportByProductIdAsync(Guid productId, DateTime? startDate = null, DateTime? endDate = null)
    {
        // Periksa produk ada
        var product = await _productRepository.GetAsync(productId);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {productId} not found");

        DateTime effectiveStartDate = startDate ?? DateTime.UtcNow.AddMonths(-1);
        DateTime effectiveEndDate = endDate ?? DateTime.UtcNow;
        
        var histories = await _inventoryHistoryRepository.GetByProductIdAsync(productId);
        // Filter berdasarkan tanggal
        histories = histories.Where(h => h.ChangedAt >= effectiveStartDate && h.ChangedAt <= effectiveEndDate);
        
        var categories = await _categoryRepository.GetAllAsync();
        var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);

        var changes = new List<StockChangeDto>();

        foreach (var history in histories)
        {
            changes.Add(new StockChangeDto
            {
                Id = history.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                CategoryName = category?.Name ?? "Category Not Found",
                PreviousQuantity = history.PreviousQuantity,
                NewQuantity = history.NewQuantity,
                QuantityChange = history.QuantityChange,
                ChangeType = history.ChangeType,
                Notes = history.Notes,
                ChangedAt = history.ChangedAt
            });
        }

        return new StockHistoryReportDto
        {
            StartDate = effectiveStartDate,
            EndDate = effectiveEndDate,
            TotalChanges = changes.Count,
            TotalAdditions = changes.Count(c => c.ChangeType == "Addition"),
            TotalReductions = changes.Count(c => c.ChangeType == "Reduction"),
            TotalAdjustments = changes.Count(c => c.ChangeType == "Adjustment"),
            Changes = changes.OrderByDescending(c => c.ChangedAt).ToList()
        };
    }
} 