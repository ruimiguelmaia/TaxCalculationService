using Application.Queries.TaxCalculation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxCalculationService.Models;

namespace TaxCalculationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaxController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// After launching the application you can use the following swagger endpoint to test it
    /// https://localhost:7110/swagger/index.html
    /// Or you test it directly using: https://localhost:7110/api/Tax/calculate?vatRate=10&netAmount=100
    /// </summary>
    /// <param name="vatRate"></param>
    /// <param name="netAmount"></param>
    /// <param name="vatAmount"></param>
    /// <param name="grossAmount"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("calculate")]
    [ProducesResponseType(typeof(TaxCalculationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessages), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaxCalculationResult>> CalculateTaxAsync(
        [FromQuery] string? vatRate,
        [FromQuery] string? netAmount,
        [FromQuery] string? vatAmount,
        [FromQuery] string? grossAmount)
    {
        var result = await _mediator.Send(new TaxCalculationQuery(
            VatRate: vatRate,
            NetAmount: netAmount,
            VatAmount: vatAmount,
            GrossAmount: grossAmount)
        );

        if (result.IsFailed)
            return BadRequest(new ErrorMessages(result.Reasons.Select(error => error.Message)));

        return Ok(result.Value);
    }
}