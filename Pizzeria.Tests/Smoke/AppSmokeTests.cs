using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pizzeria.Configuration;
using Pizzeria.Parsers;
using Pizzeria.Parsers.Csv;
using Pizzeria.Parsers.Interfaces;
using Pizzeria.Parsers.Json;
using Pizzeria.Services;
using Pizzeria.Services.Interfaces;
using Pizzeria.Tests.Utils;
using Pizzeria.Validators;

namespace Pizzeria.Tests.Smoke;

/// <summary>
/// A Smoke Test is a basic check that the application starts and runs without throwing exceptions.
/// It differs from Unit and Integration tests in that it **does not test specific logic**, 
/// but simply ensures that **all dependencies are wired up correctly and the main service runs**.
/// </summary>
public class AppSmokeTests
{
    [Fact]
    public async Task App_ShouldStartAndRunWithoutExceptions()
    {
        // Arrange: Set up minimal service collection and configuration for running the app
        var services = new ServiceCollection();

        // Load resource file paths from embedded test CSVs
        services.Configure<DataFileSettings>(opts =>
        {
            opts.Orders = TestFileHelper.GetResourcePath("orders.csv");
            opts.Products = TestFileHelper.GetResourcePath("products.csv");
            opts.Ingredients = TestFileHelper.GetResourcePath("ingredients.csv");
        });

        // Register application services
        services.AddScoped<IParserFactory, ParserFactory>();
        services.AddScoped<JsonParser>();
        services.AddScoped<CsvParser>();
        services.AddTransient<IOrderValidator, OrderValidator>();
        services.AddTransient<IOrderCalculator, OrderCalculator>();
        services.AddTransient<ISummaryPrinter, SummaryPrinter>(); // optionally replace with mock
        services.AddTransient<ISummaryResultFactory, SummaryResultFactory>();
        services.AddSingleton<IDataCacheService, DataCacheService>();
        services.AddTransient<IOrderService, OrderService>();

        // Register logging (optional, but helpful for debugging)
        services.AddLogging(cfg => cfg.AddConsole());

        var provider = services.BuildServiceProvider();

        var orderService = provider.GetRequiredService<IOrderService>();

        // Act & Assert: Just run the main method and expect no exceptions
        await orderService.RunAsync();
    }
}