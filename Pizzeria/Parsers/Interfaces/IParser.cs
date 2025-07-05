using Pizzeria.Models;

namespace Pizzeria.Parsers.Interfaces;

public interface IParser
{
    Task<IReadOnlyDictionary<string, List<Ingredient>>> ParseIngredientsAsync(string path);
    Task<IReadOnlyList<Order>> ParseOrderAsync(string path);
    Task<IReadOnlyList<Product>> ParseProductAsync(string path);
}