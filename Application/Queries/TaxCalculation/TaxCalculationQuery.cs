using FluentResults;
using MediatR;

namespace Application.Queries.TaxCalculation;

public record TaxCalculationQuery(string? VatRate,
        string? NetAmount,
        string? VatAmount,
        string? GrossAmount)
    : IRequest<Result<TaxCalculationResult>>;