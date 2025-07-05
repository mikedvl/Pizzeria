using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Parsers.Csv;
using Pizzeria.Parsers.Interfaces;
using Pizzeria.Parsers.Json;

namespace Pizzeria.Parsers;

public class ParserFactory(IServiceProvider serviceProvider) : IParserFactory
{
    public IParser GetParser(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();

        return extension switch
        {
            ".json" => serviceProvider.GetRequiredService<JsonParser>(),
            ".csv"  => serviceProvider.GetRequiredService<CsvParser>(),
            _ => throw new NotSupportedException($"Unsupported file format: {extension}")
        };
    }
}