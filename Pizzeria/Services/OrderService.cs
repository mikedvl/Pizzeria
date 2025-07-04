using Microsoft.Extensions.Logging;
using Pizzeria.Models;
using Pizzeria.Services.Interfaces;
using Pizzeria.Validators;

namespace Pizzeria.Services;

public class OrderService(
    IDataCacheService cacheService,
    IOrderValidator validator,
    IPriceCalculator priceCalculator,
    ISummaryPrinter printer,
    ILogger<OrderService> logger)
    : IOrderService
{
    public async Task RunAsync()
    {
        logger.LogInformation("Starting order processing");

        try
        {
            // Load all data through the cache
            var ordersRaw = await cacheService.GetOrdersAsync();
            var orders = ordersRaw.ToList();
            var products = await cacheService.GetProductsAsync();

            // Group and validate orders
            var grouped = orders
                .GroupBy(o => o.OrderId)
                .Select(group =>
                {
                    var items = group.ToList();
                    return validator.Validate(items, products);
                })
                .Where(v => v != null)
                .Cast<ValidatedOrder>()
                .ToList();

            logger.LogInformation("Valid orders: {Count}", grouped.Count);

            // Price calculation
            grouped.ForEach(order => priceCalculator.Calculate(order, products));

            // Print summary
            logger.LogInformation("Printing summary");
            await printer.PrintAsync(grouped);

            logger.LogInformation("Order processing completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during order processing");
        }
    }
}