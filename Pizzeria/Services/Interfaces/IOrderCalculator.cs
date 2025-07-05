using Pizzeria.Models;
using Pizzeria.Models.Dto;

namespace Pizzeria.Services.Interfaces;

public interface IOrderCalculator
{
    void Calculate(ValidatedOrder order, IReadOnlyList<Product> products);
}