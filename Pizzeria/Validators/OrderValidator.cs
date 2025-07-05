using Pizzeria.Models;

namespace Pizzeria.Validators;

public interface IOrderValidator
{
    bool IsValid(List<Order> orders, IReadOnlyList<Product> products);
}
public class OrderValidator : IOrderValidator
{
    public bool IsValid(List<Order> orders, IReadOnlyList<Product> products)
    {
        if (orders.Count == 0) return false;

        var orderId = orders[0].OrderId;

        foreach (var order in orders)
        {
            if (order.OrderId != orderId) return false;
            if (order.Quantity <= 0) return false;
            if (order.DeliveryAt < order.CreatedAt) return false;
            if (string.IsNullOrWhiteSpace(order.ProductId)) return false;
            if (products.All(p => p.ProductId != order.ProductId)) return false;
        }

        return true;
    }
}
