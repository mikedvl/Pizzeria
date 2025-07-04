using System.Text.Json;
using Pizzeria.Models;
using Pizzeria.Utils;

namespace Pizzeria.Parsers;

public interface IProductParser
{
    Task<IReadOnlyList<Product>> ParseAsync(string path, IReadOnlyDictionary<string, List<Ingredient>> ingredientMap);
}

public class ProductParser : IProductParser
{
    public async Task<IReadOnlyList<Product>> ParseAsync(string path, IReadOnlyDictionary<string, List<Ingredient>> ingredientMap)
    {
        var json = await FileLoader.ReadFileAsync(path);
        var products = JsonSerializer.Deserialize<List<Product>>(json) ?? new();

        foreach (var product in products)
        {
            if (ingredientMap.TryGetValue(product.ProductId, out var ingredients))
            {
                product.Ingredients.AddRange(ingredients);
            }
        }

        return products;
    }
}