using Application.Services;
using FluentResults;
using MediatR;

namespace Application.Queries.TaxCalculation;

public class TaxCalculationHandler : IRequestHandler<TaxCalculationQuery, Result<TaxCalculationResult>>
{
    private readonly ITaxCalculationService _taxCalculationService;

    public TaxCalculationHandler(ITaxCalculationService taxCalculationService)
    {
        _taxCalculationService = taxCalculationService;
    }

    public Task<Result<TaxCalculationResult>> Handle(TaxCalculationQuery query, CancellationToken cancellationToken)
    {
        var data = _taxCalculationService.CalculateAmounts(query);
        return Task.FromResult(Result.Ok(value: data));
    }
}