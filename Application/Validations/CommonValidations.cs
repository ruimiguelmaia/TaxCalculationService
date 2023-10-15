using Application.Queries.TaxCalculation;

namespace Application.Validations;

public static class CommonValidations
{
    private static readonly List<string> ValidVats = new() { "10", "13", "20" };

    public static bool BeAValidVat(string vat)
    {
        return ValidVats.Contains(vat);
    }

    public static bool HasJustOneAmountInput(TaxCalculationQuery query)
    {
        var count = 0;
        if (!string.IsNullOrWhiteSpace(query.NetAmount)) count++;
        if (!string.IsNullOrWhiteSpace(query.VatAmount)) count++;
        if (!string.IsNullOrWhiteSpace(query.GrossAmount)) count++;

        return count == 1;
    }

    public static bool BeAPositiveNumber(string amount)
    {
        return double.TryParse(amount, out var value) && value > 0;
    }
}