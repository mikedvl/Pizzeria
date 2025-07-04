namespace Pizzeria.Models;

public class ValidatedOrder
{
    public string OrderId { get; set; } = default!;
    public List<Order> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
    public Dictionary<string, decimal> RequiredIngredients { get; set; } = new();
}