using Application.Queries.TaxCalculation;
using Application.Services;
using AutoBogus;
using FluentAssertions;
using Xunit;

namespace Application.Test.Services;

public class TaxCalculationServiceTests
{
    [Theory]
    [InlineData("10", "100", "10", "110")]
    [InlineData("13", "200", "26", "226")]
    [InlineData("20", "250", "50", "300")]
    public void CalculateAmounts_WithValidInputs_ReturnsSuccess(string vatRate, string netAmount, string vatAmount, string grossAmount)
    {
        // Arrange
        var result = new AutoFaker<TaxCalculationResult>()
            .RuleFor(_ => _.VatRate, decimal.Parse(vatRate))
            .RuleFor(_ => _.NetAmount, decimal.Parse(netAmount))
            .RuleFor(_ => _.VatAmount, decimal.Parse(vatAmount))
            .RuleFor(_ => _.GrossAmount, decimal.Parse(grossAmount))
            .Generate();

        var service = new TaxCalculationService();

        // Act
        var responseNetAmount = service.CalculateAmounts(
            new TaxCalculationQuery(
                VatRate: vatRate,
                NetAmount: netAmount,
                VatAmount: null,
                GrossAmount: null)
        );

        var responseVatAmount = service.CalculateAmounts(
            new TaxCalculationQuery(
                VatRate: vatRate,
                NetAmount: null,
                VatAmount: vatAmount,
                GrossAmount: null)
        );

        var responseGrossAmount = service.CalculateAmounts(
            new TaxCalculationQuery(
                VatRate: vatRate,
                NetAmount: null,
                VatAmount: null,
                GrossAmount: grossAmount)
        );

        // Assert
        responseNetAmount.Should().BeEquivalentTo(result);
        responseVatAmount.Should().BeEquivalentTo(result);
        responseGrossAmount.Should().BeEquivalentTo(result);
    }
}