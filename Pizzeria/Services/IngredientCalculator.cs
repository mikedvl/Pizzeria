using Pizzeria.Models;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class IngredientCalculator : IIngredientCalculator
{
    public Dictionary<string, decimal> Calculate(List<ValidatedOrder> orders)
    {
        var total = new Dictionary<string, decimal>();

        foreach (var order in orders)
        {
            foreach (var (ingredient, amount) in order.RequiredIngredients)
            {
                total.TryAdd(ingredient, 0);
                total[ingredient] += amount;
            }
        }

        return total;
    }
}