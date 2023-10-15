namespace Application.Queries.TaxCalculation;

public record TaxCalculationResult(decimal VatRate, decimal NetAmount, decimal VatAmount, decimal GrossAmount);