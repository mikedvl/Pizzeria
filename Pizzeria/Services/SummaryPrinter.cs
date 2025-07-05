using System.Globalization;
using Pizzeria.Models.Dto;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class SummaryPrinter : ISummaryPrinter
{
    public void Print(SummaryResult summary)
    {
        var usdCulture = new CultureInfo("en-US");

        PrintOverview(summary.ValidatedOrders);
        PrintOrderDetails(summary.ValidatedOrders, usdCulture);
        PrintTotalIngredients(summary, usdCulture);
    }

    private static void PrintOverview(List<ValidatedOrder> orders)
    {
        var rawEntryCount = orders.SelectMany(o => o.Items).Count();
        var uniqueOrderCount = orders.Count;
        var totalPizzaCount = orders.SelectMany(o => o.Items).Sum(i => i.Quantity);

        Console.WriteLine("===== SUMMARY OVERVIEW =====\n");
        Console.WriteLine($"Raw order entries (lines in file): {rawEntryCount}");
        Console.WriteLine($"Unique order IDs: {uniqueOrderCount}");
        Console.WriteLine($"Total pizza items (by quantity): {totalPizzaCount}\n");
    }

    private static void PrintOrderDetails(List<ValidatedOrder> orders, CultureInfo usdCulture)
    {
        Console.WriteLine("===== VALID ORDERS SUMMARY =====\n");

        foreach (var order in orders)
        {
            var quantity = order.Items.Sum(i => i.Quantity);

            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine($"CreatedAt: {order.Items.First().CreatedAt}");
            Console.WriteLine("will be delivered to {0}", order.Items[0].DeliveryAddress);
            Console.WriteLine($"DeliveryAt: {order.Items.First().DeliveryAt}");
            Console.WriteLine($"Items: {quantity}");

            foreach (var item in order.Items)
                Console.WriteLine($" - {item.ProductName} x {item.Quantity}");

            Console.WriteLine($"Total Price: {order.TotalPrice.ToString("C", usdCulture)}");
            Console.WriteLine("Ingredients:");

            foreach (var (name, amount) in order.RequiredIngredients)
                Console.WriteLine($"  - {name}: {amount}");

            Console.WriteLine("-------------------------------\n");
        }
    }

    private static void PrintTotalIngredients(SummaryResult summary, CultureInfo usdCulture)
    {
        Console.WriteLine("===== TOTAL INGREDIENTS REQUIRED =====\n");

        foreach (var (name, amount) in summary.TotalIngredients)
            Console.WriteLine($"{name}: {amount}");

        Console.WriteLine($"\nTotal Price Across All Orders: {summary.TotalPrice.ToString("C", usdCulture)}");
        Console.WriteLine("\n===== END OF SUMMARY =====");
    }
}