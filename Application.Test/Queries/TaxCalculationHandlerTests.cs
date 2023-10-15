using Application.Queries.TaxCalculation;
using Application.Services;
using AutoBogus;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Queries;

public class TaxCalculationHandlerTests
{
    [Fact]
    public async Task Handle_CalculateAmounts_ReturnsSuccess()
    {
        // Arrange
        var taxCalculationQuery = new AutoFaker<TaxCalculationQuery>().Generate();
        var taxCalculationResult = new AutoFaker<TaxCalculationResult>().Generate();

        var taxCalculationService = new Mock<ITaxCalculationService>();
        taxCalculationService
            .Setup(x => x.CalculateAmounts(It.IsAny<TaxCalculationQuery>()))
            .Returns(taxCalculationResult);

        var handler = new TaxCalculationHandler(taxCalculationService: taxCalculationService.Object);

        // Act
        var result = await handler.Handle(query: taxCalculationQuery, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        Assert.True(result.IsSuccess);
    }
}