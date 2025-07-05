using Pizzeria.Parsers.Csv;
using Pizzeria.Tests.Utils;

namespace Pizzeria.Tests.Unit.Parsers;

public class CsvParserTests
{
    private readonly CsvParser _parser = new();

    [Fact]
    public async Task ParseOrderAsync_FromEmbeddedFile_ShouldReturnValidOrders()
    {
        var path = TestFileHelper.GetResourcePath("orders.csv");

        var orders = await _parser.ParseOrderAsync(path);

        Assert.NotEmpty(orders);
        Assert.All(orders, o => Assert.False(string.IsNullOrWhiteSpace(o.OrderId)));
    }

    [Fact]
    public async Task ParseProductAsync_FromEmbeddedFile_ShouldReturnValidProducts()
    {
        var path = TestFileHelper.GetResourcePath("products.csv");

        var products = await _parser.ParseProductAsync(path);

        Assert.NotEmpty(products);
        Assert.All(products, p => Assert.True(p.Price > 0));
    }

    [Fact]
    public async Task ParseIngredientsAsync_FromEmbeddedFile_ShouldReturnValidMap()
    {
        var path = TestFileHelper.GetResourcePath("ingredients.csv");

        var ingredients = await _parser.ParseIngredientsAsync(path);

        Assert.NotEmpty(ingredients);
        Assert.All(ingredients.Values.SelectMany(i => i), i => Assert.True(i.Amount > 0));
    }
}