using Application.Queries.TaxCalculation;

namespace Application.Services;

public class TaxCalculationService : ITaxCalculationService
{
    public TaxCalculationResult CalculateAmounts(TaxCalculationQuery request)
    {
        var vatRate = decimal.Parse(request.VatRate);
        decimal netAmount = 0;
        decimal vatAmount = 0;
        decimal grossAmount = 0;

        // NET
        if (!string.IsNullOrWhiteSpace(request.NetAmount))
        {
            netAmount = decimal.Parse(request.NetAmount);
            vatAmount = netAmount * (vatRate / 100);
            grossAmount = netAmount + vatAmount;
        }
        // VAT
        else if (!string.IsNullOrWhiteSpace(request.VatAmount))
        {
            vatAmount = decimal.Parse(request.VatAmount);
            grossAmount = vatAmount + (vatAmount / (vatRate / 100));
            netAmount = grossAmount - vatAmount;
        }
        // GROSS
        else if (!string.IsNullOrWhiteSpace(request.GrossAmount))
        {
            grossAmount = decimal.Parse(request.GrossAmount);
            netAmount = grossAmount / (1 + (vatRate / 100));
            vatAmount = grossAmount - netAmount;
        }

        return new TaxCalculationResult(
            VatRate: vatRate,
            NetAmount: netAmount,
            VatAmount: vatAmount,
            GrossAmount: grossAmount);
    }
}