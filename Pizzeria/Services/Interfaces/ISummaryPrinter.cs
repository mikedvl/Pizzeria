using Pizzeria.Models.Dto;

namespace Pizzeria.Services.Interfaces;

public interface ISummaryPrinter
{
    public void Print(SummaryResult summary);
}