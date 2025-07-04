using Pizzeria.Models;

namespace Pizzeria.Services.Interfaces;

public interface IIngredientCalculator
{
    Dictionary<string, decimal> Calculate(List<ValidatedOrder> orders);
}