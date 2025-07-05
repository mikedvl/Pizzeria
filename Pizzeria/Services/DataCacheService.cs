using Microsoft.Extensions.Options;
using Pizzeria.Configuration;
using Pizzeria.Models;
using Pizzeria.Parsers.Interfaces;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class DataCacheService(
    IOptions<DataFileSettings> options,
    IParserFactory parserFactory
) : IDataCacheService
{
    private readonly DataFileSettings _paths = options.Value;

    private IReadOnlyList<Order>? _orders;
    private IReadOnlyList<Product>? _products;
    private IReadOnlyDictionary<string, List<Ingredient>>? _ingredients;

    public async Task<IReadOnlyList<Order>> GetOrdersAsync()
    {
        if (_orders != null)
            return _orders;

        var parser = parserFactory.GetParser(_paths.Orders);
        _orders = await parser.ParseOrderAsync(_paths.Orders);
        return _orders;
    }

    public async Task<IReadOnlyDictionary<string, List<Ingredient>>> GetIngredientsAsync()
    {
        if (_ingredients != null)
            return _ingredients;

        var parser = parserFactory.GetParser(_paths.Ingredients);
        _ingredients = await parser.ParseIngredientsAsync(_paths.Ingredients);
        return _ingredients;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        if (_products != null)
            return _products;

        var ingredients = await GetIngredientsAsync();

        var parser = parserFactory.GetParser(_paths.Products);
        var products = await parser.ParseProductAsync(_paths.Products);

        foreach (var product in products)
        {
            if (ingredients.TryGetValue(product.ProductId, out var productIngredients))
            {
                product.Ingredients.AddRange(productIngredients);
            }
        }

        _products = products;
        return _products;
    }
}