namespace Pizzeria.Models.Dto;

public record SummaryResult(
    List<ValidatedOrder> ValidatedOrders,
    Dictionary<string, decimal> TotalIngredients,
    decimal TotalPrice
);