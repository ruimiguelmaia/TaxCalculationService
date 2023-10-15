using Application.Queries.TaxCalculation;
using Bogus;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Test.Queries;

public class TaxCalculationValidatorTests
{
    private static readonly List<string> ValidVats = new() { "10", "13", "20" };
    private const string InvalidVat = "17";

    [Fact]
    public void TaxCalculationValidator_HappyFlowNetAmount_Success()
    {
        // Arrange
        var validator = new TaxCalculationValidator();
        var query = new TaxCalculationQuery(
            VatRate: new Faker().PickRandom(ValidVats),
            NetAmount: GeneratePositiveNumber(),
            VatAmount: null,
            GrossAmount: null
        );

        // Act
        var result = validator.TestValidate(objectToTest: query, strategy => strategy.IncludeAllRuleSets());

        // Assert
        Assert.True(condition: result.IsValid);
    }

    [Fact]
    public void TaxCalculationValidator_InvalidTaxRate_ShouldFail()
    {
        // Arrange
        var validator = new TaxCalculationValidator();
        var query = new TaxCalculationQuery(
            VatRate: InvalidVat,
            NetAmount: null,
            VatAmount: null,
            GrossAmount: null
        );

        // Act
        var result = validator.TestValidate(objectToTest: query, strategy => strategy.IncludeAllRuleSets());

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VatRate);
    }

    [Fact]
    public void TaxCalculationValidator_InvalidNumberOfAmountInputs_ShouldFail()
    {
        // Arrange
        var validator = new TaxCalculationValidator();
        var query = new TaxCalculationQuery(
            VatRate: new Faker().PickRandom(ValidVats),
            NetAmount: null,
            VatAmount: GeneratePositiveNumber(),
            GrossAmount: GeneratePositiveNumber()
        );

        // Act
        var result = validator.TestValidate(objectToTest: query, strategy => strategy.IncludeAllRuleSets());

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void TaxCalculationValidator_InvalidAmountInput_ShouldFail()
    {
        // Arrange
        var validator = new TaxCalculationValidator();
        var query = new TaxCalculationQuery(
            VatRate: new Faker().PickRandom(ValidVats),
            NetAmount: GenerateNegativeNumber(),
            VatAmount: null,
            GrossAmount: null
        );

        // Act
        var result = validator.TestValidate(objectToTest: query, strategy => strategy.IncludeAllRuleSets());

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.NetAmount);
    }


    private static string GeneratePositiveNumber()
    {
        return new Faker().Random.Decimal(min: 1, max: 100).ToString("0.00");
    }

    private static string GenerateNegativeNumber()
    {
        return new Faker().Random.Decimal(min: -100, max: -1).ToString("0.00");
    }
}