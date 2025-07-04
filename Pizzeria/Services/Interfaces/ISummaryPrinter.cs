using Pizzeria.Models;

namespace Pizzeria.Services.Interfaces;

public interface ISummaryPrinter
{
    Task PrintAsync(List<ValidatedOrder> orders);
}