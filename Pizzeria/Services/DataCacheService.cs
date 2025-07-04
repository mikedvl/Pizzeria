using Microsoft.Extensions.Options;
using Pizzeria.Configuration;
using Pizzeria.Models;
using Pizzeria.Parsers;
using Pizzeria.Services.Interfaces;

namespace Pizzeria.Services;

public class DataCacheService(IOptions<DataFileSettings> options, 
    IOrderParser orderParser,
    IIngredientParser ingredientParser,
    IProductParser productParser) : IDataCacheService
{
    private readonly DataFileSettings _paths = options.Value;

    private IReadOnlyList<Order>? _orders;
    private IReadOnlyList<Product>? _products;
    private IReadOnlyDictionary<string, List<Ingredient>>? _ingredients;
    

    public async Task<IReadOnlyList<Order>> GetOrdersAsync()
    {
        if (_orders != null)
            return _orders;

        _orders = await orderParser.ParseAsync(_paths.Orders);
        return _orders;
    }


    public async Task<IReadOnlyDictionary<string, List<Ingredient>>> GetIngredientsAsync()
    {
        if (_ingredients != null)
            return _ingredients;

        _ingredients = await ingredientParser.ParseAsync(_paths.Ingredients);
        return _ingredients;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        if (_products != null)
            return _products;

        var ingredients = await GetIngredientsAsync();
        _products = await productParser.ParseAsync(_paths.Products, ingredients);
        return _products;
    }
}