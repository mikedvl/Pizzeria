using Pizzeria.Models;
using Pizzeria.Models.Dto;
using Pizzeria.Services;

namespace Pizzeria.Tests.Unit.Calculators;

public class OrderCalculatorTests
{
    private readonly OrderCalculator _calculator = new();

    [Fact]
    public void Calculate_WithValidItems_SetsTotalPriceAndIngredients()
    {
        // Arrange
        var order = new ValidatedOrder
        {
            OrderId = "ORD1",
            Items = [new OrderDisplayItem("PIZZA1", "Margherita", 2, DateTime.Now, 
                DateTime.Now.AddHours(1), "123 Main St")]
        };

        var products = new List<Product>
        {
            new("PIZZA1", "Margherita", 10.0m)
            {
                Ingredients =
                [
                    new("Tomato", 0.5m),
                    new("Cheese", 0.3m)
                ]
            }
        };

        // Act
        _calculator.Calculate(order, products);

        // Assert
        Assert.Equal(20.0m, order.TotalPrice);
        Assert.Equal(2, order.RequiredIngredients.Count);
        Assert.Equal(1.0m, order.RequiredIngredients["Tomato"]);
        Assert.Equal(0.6m, order.RequiredIngredients["Cheese"]);
    }

    [Fact]
    public void CalculateTotalIngredients_WithMultipleOrders_AggregatesCorrectly()
    {
        // Arrange
        var orders = new List<ValidatedOrder>
        {
            new()
            {
                OrderId = "ORD1",
                RequiredIngredients = new Dictionary<string, decimal>
                {
                    { "Tomato", 1.0m },
                    { "Cheese", 0.6m }
                }
            },
            new()
            {
                OrderId = "ORD2",
                RequiredIngredients = new Dictionary<string, decimal>
                {
                    { "Tomato", 0.5m },
                    { "Pepperoni", 0.7m }
                }
            }
        };

        // Act
        var result = _calculator.CalculateTotalIngredients(orders);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1.5m, result["Tomato"]);
        Assert.Equal(0.6m, result["Cheese"]);
        Assert.Equal(0.7m, result["Pepperoni"]);
    }
}