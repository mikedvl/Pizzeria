using System.Globalization;
using Pizzeria.Models;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class SummaryPrinter(IDataCacheService cacheService) : ISummaryPrinter
{
    public async Task PrintAsync(List<ValidatedOrder> orders)
    {
        // Use US dollar formatting
        var usdCulture = new CultureInfo("en-US");

        // === SUMMARY OVERVIEW ===
        var rawEntryCount = orders.SelectMany(o => o.Items).Count();
        var uniqueOrderCount = orders.Count;
        var totalPizzaCount = orders
            .SelectMany(o => o.Items)
            .Sum(i => i.Quantity);

        Console.WriteLine("===== SUMMARY OVERVIEW =====\n");
        Console.WriteLine($"Raw order entries (lines in file): {rawEntryCount}");
        Console.WriteLine($"Unique order IDs: {uniqueOrderCount}");
        Console.WriteLine($"Total pizza items (by quantity): {totalPizzaCount}\n");

        // === VALID ORDERS DETAIL ===
        Console.WriteLine("===== VALID ORDERS SUMMARY =====\n");

        foreach (var order in orders)
        {
            var quantity = order.Items.Sum(i => i.Quantity);

            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine("will be delivered to {0}", order.Items[0].DeliveryAddress);
            Console.WriteLine($"Items: {quantity}");
            
            
            //переписать так как в заказе можно хранить сам продукт
            var products = await cacheService.GetProductsAsync();
            var productMap = products.ToDictionary(p => p.ProductId);

            foreach (var item in order.Items)
            {
                if (productMap.TryGetValue(item.ProductId, out var product))
                {
                    Console.WriteLine($" - {product.ProductName} x {item.Quantity}");
                }
                else
                {
                    Console.WriteLine($" - Unknown product ({item.ProductId}) x {item.Quantity}");
                }
            }
            //
            
            Console.WriteLine($"Total Price: {order.TotalPrice.ToString("C", usdCulture)}");
            Console.WriteLine("Ingredients:");
            foreach (var (name, amount) in order.RequiredIngredients)
            {
                Console.WriteLine($"  - {name}: {amount}");
            }
            Console.WriteLine("-------------------------------\n");
        }

        // === TOTAL INGREDIENTS ACROSS ALL ORDERS ===
        Console.WriteLine("===== TOTAL INGREDIENTS REQUIRED =====\n");

        var totalIngredients = new Dictionary<string, decimal>();

        foreach (var order in orders)
        {
            foreach (var (name, amount) in order.RequiredIngredients)
            {
                totalIngredients.TryAdd(name, 0);
                totalIngredients[name] += amount;
            }
        }

        foreach (var (name, amount) in totalIngredients)
        {
            Console.WriteLine($"{name}: {amount}");
        }

        Console.WriteLine("\n===== END OF SUMMARY =====");
    }
}