using System.Text.Json;
using Pizzeria.Models;
using Pizzeria.Utils;

namespace Pizzeria.Parsers;

public interface IIngredientParser
{
    Task<IReadOnlyDictionary<string, List<Ingredient>>> ParseAsync(string path);
}

public class IngredientParser : IIngredientParser
{
    public async Task<IReadOnlyDictionary<string, List<Ingredient>>> ParseAsync(string path)
    {
        var json = await FileLoader.ReadFileAsync(path);
        return JsonSerializer.Deserialize<Dictionary<string, List<Ingredient>>>(json)
               ?? new Dictionary<string, List<Ingredient>>();
    }
}