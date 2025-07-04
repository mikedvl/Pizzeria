using Pizzeria.Models;

namespace Pizzeria.Validators;

public interface IOrderValidator
{
    ValidatedOrder? Validate(List<Order> orders, IReadOnlyList<Product> products);
}

public class OrderValidator : IOrderValidator
{
    public ValidatedOrder? Validate(List<Order> orders, IReadOnlyList<Product> products)
    {
        if (orders.Count == 0)
            return null;

        var orderId = orders.First().OrderId;

        // Простейшие проверки: одинаковые OrderId, положительное количество, валидные даты и продукт
        foreach (var order in orders)
        {
            if (order.OrderId != orderId)
                return null;

            if (order.Quantity <= 0)
                return null;

            if (order.DeliveryAt < order.CreatedAt)
                return null;

            if (string.IsNullOrWhiteSpace(order.ProductId))
                return null;

            if (products.All(p => p.ProductId != order.ProductId))
                return null;
        }

        return new ValidatedOrder
        {
            OrderId = orderId,
            Items = orders
        };
    }
}