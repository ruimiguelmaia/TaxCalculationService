using Application.Queries.TaxCalculation;
using AutoBogus;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaxCalculationService.Controllers;
using Xunit;

namespace TaxCalculationService.Test;

public class TaxCalculationTest
{
    [Fact]
    public async Task CalculateTax_WhenCalculationSucceeds_ReturnsOkResult()
    {
        // Arrange
        var taxResult = new AutoFaker<TaxCalculationResult>().Generate();

        var mediator = new Mock<IMediator>();
        var controller = new TaxController(mediator: mediator.Object);

        mediator
            .Setup(x => x.Send(It.IsAny<TaxCalculationQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(taxResult));

        // Act
        var response = await controller.CalculateTaxAsync("", "", "", "");

        // Assert
        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async Task CalculateTax_WhenCalculationFails_ReturnsBadRequestResult()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var controller = new TaxController(mediator: mediator.Object);

        mediator
            .Setup(x => x.Send(It.IsAny<TaxCalculationQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Fail("Error message"));

        // Act
        var response = await controller.CalculateTaxAsync("", "", "", "");

        // Assert
        Assert.IsType<BadRequestObjectResult>(response.Result);
    }
}