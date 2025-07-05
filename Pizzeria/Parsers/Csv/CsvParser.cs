using System.Globalization;
using Pizzeria.Models;
using Pizzeria.Parsers.Interfaces;
using Pizzeria.Utils;

namespace Pizzeria.Parsers.Csv;

public class CsvParser : IParser
{
    public async Task<IReadOnlyDictionary<string, List<Ingredient>>> ParseIngredientsAsync(string path)
    {
        EnsureCsvExtension(path);

        return await ParseWithHeaderAsync(path, (parts, map) =>
        {
            if (parts.Length < 3)
                return;

            var productId = parts[0].Trim();
            var ingredientName = parts[1].Trim();
            if (!decimal.TryParse(parts[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                return;

            if (!map.TryGetValue(productId, out var list))
            {
                list = [];
                map[productId] = list;
            }

            list.Add(new Ingredient(ingredientName, amount));
        });
    }

    public async Task<IReadOnlyList<Order>> ParseOrderAsync(string path)
    {
        EnsureCsvExtension(path);

        return await ParseListAsync(path, parts =>
        {
            if (parts.Length < 6)
                return null;

            if (!int.TryParse(parts[2].Trim(), out var quantity))
                return null;

            if (!DateTime.TryParse(parts[3].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var createdAt))
                return null;

            if (!DateTime.TryParse(parts[4].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var deliveryAt))
                return null;

            return new Order(
                OrderId: parts[0].Trim(),
                ProductId: parts[1].Trim(),
                Quantity: quantity,
                CreatedAt: createdAt,
                DeliveryAt: deliveryAt,
                DeliveryAddress: parts[5].Trim()
            );
        });
    }

    public async Task<IReadOnlyList<Product>> ParseProductAsync(string path)
    {
        EnsureCsvExtension(path);

        return await ParseListAsync(path, parts =>
        {
            if (parts.Length < 3)
                return null;

            return !decimal.TryParse(parts[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var price) ? 
                null : new Product(parts[0].Trim(), parts[1].Trim(), price);
        });
    }

    private static void EnsureCsvExtension(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        if (extension != ".csv")
            throw new InvalidOperationException("Only CSV format is supported.");
    }

    private static async Task<IReadOnlyList<T>> ParseListAsync<T>(string path, Func<string[], T?> factory)
        where T : class
    {
        var result = new List<T>();
        using var reader = new StreamReader(path);
        var isFirstLine = true;
        var delimiter = ',';

        while (await reader.ReadLineAsync() is { } line)
        {
            if (isFirstLine)
            {
                delimiter = CsvUtils.DetectDelimiter(line);
                isFirstLine = false;
                continue;
            }

            var parts = line.Split(delimiter);
            var item = factory(parts);
            if (item != null)
                result.Add(item);
        }

        return result;
    }

    private static async Task<IReadOnlyDictionary<string, List<Ingredient>>> ParseWithHeaderAsync(
        string path,
        Action<string[], Dictionary<string, List<Ingredient>>> lineProcessor)
    {
        var result = new Dictionary<string, List<Ingredient>>(StringComparer.OrdinalIgnoreCase);
        using var reader = new StreamReader(path);
        var isFirstLine = true;
        var delimiter = ',';

        while (await reader.ReadLineAsync() is { } line)
        {
            if (isFirstLine)
            {
                delimiter = CsvUtils.DetectDelimiter(line);
                isFirstLine = false;
                continue;
            }

            var parts = line.Split(delimiter);
            lineProcessor(parts, result);
        }

        return result;
    }
}