using Pizzeria.Models;
using Pizzeria.Models.Dto;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class OrderCalculator : IOrderCalculator
{
    public void Calculate(ValidatedOrder order, IReadOnlyList<Product> products)
    {
        decimal total = 0;

        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product == null) continue;

            total += product.Price * item.Quantity;

            foreach (var ing in product.Ingredients)
            {
                if (!order.RequiredIngredients.TryAdd(ing.Name, 0))
                {
                    order.RequiredIngredients[ing.Name] += ing.Amount * item.Quantity;
                }
                else
                {
                    order.RequiredIngredients[ing.Name] = ing.Amount * item.Quantity;
                }
            }
        }

        order.TotalPrice = total;
    }

    public Dictionary<string, decimal> CalculateTotalIngredients(List<ValidatedOrder> orders)
    {
        var totalIngredients = new Dictionary<string, decimal>();

        foreach (var order in orders)
        {
            foreach (var (name, amount) in order.RequiredIngredients)
            {
                if (!totalIngredients.TryAdd(name, amount))
                {
                    totalIngredients[name] += amount;
                }
            }
        }

        return totalIngredients;
    }
}