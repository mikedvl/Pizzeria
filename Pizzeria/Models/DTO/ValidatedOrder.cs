namespace Pizzeria.Models.Dto;

public class ValidatedOrder
{
    public string OrderId { get; init; } = default!;
    public List<OrderDisplayItem> Items { get; init; } = new();
    public decimal TotalPrice { get; set; }
    public Dictionary<string, decimal> RequiredIngredients { get; set; } = new();
}