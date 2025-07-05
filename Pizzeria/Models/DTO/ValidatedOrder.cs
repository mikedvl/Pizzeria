namespace Pizzeria.Models.Dto;

public class ValidatedOrder
{
    public string OrderId { get; init; } = string.Empty;
    public List<OrderDisplayItem> Items { get; init; } = [];
    public decimal TotalPrice { get; set; }
    public Dictionary<string, decimal> RequiredIngredients { get; init; } = new();
}