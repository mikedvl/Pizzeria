using System.Text.Json;
using Pizzeria.Utils;

namespace Pizzeria.Parsers.Json;

public class GenericJsonParser
{
    protected async Task<IReadOnlyDictionary<string, List<T>>> ParseMapAsync<T>(string path) where T : class
    {
        EnsureJsonExtension(path);

        var json = await FileLoader.ReadFileAsync(path);
        return JsonSerializer.Deserialize<Dictionary<string, List<T>>>(json)
               ?? new Dictionary<string, List<T>>();
    }

    protected async Task<IReadOnlyList<T>> ParseListAsync<T>(string path) where T : class
    {
        EnsureJsonExtension(path);

        var json = await FileLoader.ReadFileAsync(path);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    private static void EnsureJsonExtension(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        if (extension != ".json")
            throw new InvalidOperationException("Only JSON format is supported.");
    }
}