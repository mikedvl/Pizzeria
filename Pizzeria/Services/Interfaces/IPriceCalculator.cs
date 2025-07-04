using Pizzeria.Models;

namespace Pizzeria.Services.Interfaces;

public interface IPriceCalculator
{
    void Calculate(ValidatedOrder order, IReadOnlyList<Product> products);
}