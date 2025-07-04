using System.Text.Json;
using Pizzeria.Models;
using Pizzeria.Utils;

namespace Pizzeria.Parsers;

public interface IOrderParser
{
    Task<IReadOnlyList<Order>> ParseAsync(string path);
}

public class OrderParser : IOrderParser
{
    public async Task<IReadOnlyList<Order>> ParseAsync(string path)
    {
        var extension = Path.GetExtension(path).ToLower();

        if (extension != ".json")
            throw new InvalidOperationException("Only JSON format is supported.");

        var json = await FileLoader.ReadFileAsync(path);
        return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
    }
}