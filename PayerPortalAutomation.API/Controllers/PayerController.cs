using Microsoft.AspNetCore.Mvc;
using PayerPortalAutomation.Application.DTOs;
using PayerPortalAutomation.Application.Interfaces;

namespace PayerPortalAutomation.API.Controllers;

[ApiController]
[Route("api/payer")]
public class PayerController : ControllerBase
{
    private readonly IPayerAutomationService _service;

    public PayerController(IPayerAutomationService service)
    {
        _service = service;
    }

    [HttpPost("eobs")]
    public async Task<IActionResult> RetrieveEobs([FromBody] EobRequest request)
    {
        try
        {
            var result = await _service.RetrieveEobsAsync(request);

            if (result == null)
                return BadRequest($"No payer found for payerId={request.PayerId}, taxId={request.TaxId}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPost("submit-appeal")]
    public async Task<IActionResult> SubmitAppeal([FromBody] AppealRequest request)
    {
        try
        {
            var result = await _service.SubmitAppealAsync(request);

            if (result == null)
                return BadRequest($"No payer found for payerId={request.PayerId}, taxId={request.TaxId}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}