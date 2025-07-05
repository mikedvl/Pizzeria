using Pizzeria.Models;
using Pizzeria.Models.Dto;

namespace Pizzeria.Services.Interfaces;

public interface ISummaryResultFactory
{
    SummaryResult Create(List<Order> orders, IReadOnlyList<Product> products);
}