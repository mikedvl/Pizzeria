using Pizzeria.Models;
using Pizzeria.Models.Dto;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class OrderCalculator : IOrderCalculator
{
    public void Calculate(ValidatedOrder order, IReadOnlyList<Product> products)
    {
        decimal total = 0;
        var ingredients = new Dictionary<string, decimal>();

        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product == null) continue;

            total += product.Price * item.Quantity;

            foreach (var ing in product.Ingredients)
            {
                ingredients.TryAdd(ing.Name, 0);
                ingredients[ing.Name] += ing.Amount * item.Quantity;
            }
        }

        order.TotalPrice = total;
        order.RequiredIngredients = ingredients;
    }
    
    public Dictionary<string, decimal> CalculateTotalIngredients(List<ValidatedOrder> orders)
    {
        var totalIngredients = new Dictionary<string, decimal>();
        
        foreach (var order in orders)
        {
            foreach (var (name, amount) in order.RequiredIngredients)
            {
                totalIngredients.TryAdd(name, 0);
                totalIngredients[name] += amount;
            }
        }

        return totalIngredients;
    }
}