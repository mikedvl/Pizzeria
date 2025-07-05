using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Pizzeria.Configuration;
using Pizzeria.Services;
using Pizzeria.Parsers;
using Pizzeria.Parsers.Csv;
using Pizzeria.Parsers.Interfaces;
using Pizzeria.Parsers.Json;
using Pizzeria.Services.Interfaces;
using Pizzeria.Validators;


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

        services.AddScoped<CsvParser>();
        services.AddScoped<JsonParser>();
        services.AddScoped<IParserFactory, ParserFactory>();

        services.AddTransient<IOrderValidator, OrderValidator>();
        services.AddTransient<IOrderCalculator, OrderCalculator>();
        services.AddTransient<ISummaryPrinter, SummaryPrinter>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddSingleton<IDataCacheService, DataCacheService>();
        services.AddTransient<ISummaryResultFactory, SummaryResultFactory>();
    });

var host = builder.Build();

var app = host.Services.GetRequiredService<IOrderService>();
await app.RunAsync();