using Pizzeria.Models;

namespace Pizzeria.Services.Interfaces;

public interface IDataCacheService
{
    Task<IReadOnlyList<Order>> GetOrdersAsync();
    Task<IReadOnlyList<Product>> GetProductsAsync();
    Task<IReadOnlyDictionary<string, List<Ingredient>>> GetIngredientsAsync();
}