using Microsoft.Extensions.Logging;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class OrderService(
    IDataCacheService cacheService,
    ISummaryResultFactory summaryResultFactory,
    ISummaryPrinter printer,
    ILogger<OrderService> logger)
    : IOrderService
{
    public async Task RunAsync()
    {
        logger.LogInformation("Starting order processing");

        try
        {
            var orders = (await cacheService.GetOrdersAsync()).ToList();
            var products = await cacheService.GetProductsAsync();

            var summary = summaryResultFactory.Create(orders, products);

            logger.LogInformation("Valid orders: {Count}", summary.ValidatedOrders.Count);

            printer.Print(summary);

            logger.LogInformation("Order processing completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during order processing");
        }
    }
}