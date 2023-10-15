using Application.Queries.TaxCalculation;

namespace Application.Services;

public interface ITaxCalculationService
{
    TaxCalculationResult CalculateAmounts(TaxCalculationQuery request);
}