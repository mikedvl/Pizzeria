namespace Pizzeria.Models;

public record Order(
    string OrderId,
    string ProductId,
    int Quantity,
    DateTime CreatedAt,
    DateTime DeliveryAt,
    string DeliveryAddress
);