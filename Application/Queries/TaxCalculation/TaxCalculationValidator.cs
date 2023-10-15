using Application.Validations;
using FluentValidation;

namespace Application.Queries.TaxCalculation;

public class TaxCalculationValidator : AbstractValidator<TaxCalculationQuery>
{
    public TaxCalculationValidator()
    {
        RuleFor(x => x.VatRate)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("Vat Rate should be provided")
            .Must(CommonValidations.BeAValidVat).WithMessage("Invalid Vat Rate");

        RuleFor(x => x)
            .Must(CommonValidations.HasJustOneAmountInput)
            .WithMessage("One (and only one) amount input should be provided")
            .DependentRules(() => {
                When(x => !string.IsNullOrWhiteSpace(x.NetAmount),
                    () => RuleFor(x => x.NetAmount)
                        .Must(CommonValidations.BeAPositiveNumber)
                        .WithMessage("Invalid NetAmount. It should be a positive amount"));

                When(x => !string.IsNullOrWhiteSpace(x.VatAmount),
                    () => RuleFor(x => x.VatAmount)
                        .Must(CommonValidations.BeAPositiveNumber)
                        .WithMessage("Invalid VatAmount. It should be a positive amount"));

                When(x => !string.IsNullOrWhiteSpace(x.GrossAmount),
                    () => RuleFor(x => x.GrossAmount)
                        .Must(CommonValidations.BeAPositiveNumber)
                        .WithMessage("Invalid GrossAmount. It should be a positive amount"));
            });
    }
}