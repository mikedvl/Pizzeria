using Pizzeria.Models;
using Pizzeria.Models.Dto;
using Pizzeria.Services.Interfaces;
using Pizzeria.Validators;

namespace Pizzeria.Services;

public class SummaryResultFactory(
    IOrderValidator validator,
    IOrderCalculator orderCalculator)
    : ISummaryResultFactory
{
    public SummaryResult Create(List<Order> orders, IReadOnlyList<Product> products)
    {
        var grouped = orders
            .GroupBy(o => o.OrderId)
            .ToList();

        var validatedOrders = new List<ValidatedOrder>();

        foreach (var group in grouped)
        {
            var items = group.ToList();

            if (!validator.IsValid(items, products))
                continue;

            var productMap = products.ToDictionary(p => p.ProductId);
            var displayItems = items
                .Where(o => productMap.ContainsKey(o.ProductId))
                .Select(o => new OrderDisplayItem(
                    ProductId: o.ProductId,
                    ProductName: productMap[o.ProductId].ProductName,
                    Quantity: o.Quantity,
                    CreatedAt: o.CreatedAt,
                    DeliveryAt: o.DeliveryAt,
                    DeliveryAddress: o.DeliveryAddress
                ))
                .ToList();

            var validated = new ValidatedOrder
            {
                OrderId = group.Key,
                Items = displayItems
            };

            orderCalculator.Calculate(validated, products);

            validatedOrders.Add(validated);
        }

        var totalIngredients = new Dictionary<string, decimal>();
        decimal totalPrice = 0;

        foreach (var order in validatedOrders)
        {
            totalPrice += order.TotalPrice;

            foreach (var (name, amount) in order.RequiredIngredients)
            {
                totalIngredients.TryAdd(name, 0);
                totalIngredients[name] += amount;
            }
        }

        return new SummaryResult(
            ValidatedOrders: validatedOrders,
            TotalIngredients: totalIngredients,
            TotalPrice: totalPrice
        );
    }
}