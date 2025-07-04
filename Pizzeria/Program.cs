using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Pizzeria.Configuration;
using Pizzeria.Services;
using Pizzeria.Parsers;
using Pizzeria.Services.Interfaces;
using Pizzeria.Validators;

namespace Pizzeria;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<DataFileSettings>(context.Configuration.GetSection("DataFiles"));

                services.AddTransient<IOrderParser, OrderParser>();
                services.AddTransient<IProductParser, ProductParser>();
                services.AddTransient<IIngredientParser, IngredientParser>();
                services.AddTransient<IOrderValidator, OrderValidator>();
                services.AddTransient<IPriceCalculator, PriceCalculator>();
                services.AddTransient<IIngredientCalculator, IngredientCalculator>();
                services.AddTransient<ISummaryPrinter, SummaryPrinter>();
                services.AddTransient<IOrderService, OrderService>();
                services.AddSingleton<IDataCacheService, DataCacheService>();
            });

        var host = builder.Build();

        var app = host.Services.GetRequiredService<IOrderService>();
        await app.RunAsync();
    }
}