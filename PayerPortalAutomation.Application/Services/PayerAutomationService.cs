using PayerPortalAutomation.Application.DTOs;
using PayerPortalAutomation.Application.Interfaces;
using PayerPortalAutomation.Application.Repositories;
using PayerPortalAutomation.Domain.Entities;

public class PayerAutomationService : IPayerAutomationService
{
    private readonly IPortalAutomation _automation;
    private readonly IPayerTargetRepository _repository;

    public PayerAutomationService(
        IPortalAutomation automation,
        IPayerTargetRepository repository)
    {
        _automation = automation;
        _repository = repository;
    }

    public async Task<EobRunResult> RetrieveEobsAsync(EobRequest request)
    {
        var payer = await _repository.GetAsync(
            request.PayerId,
            request.TaxId);

        var runId = Guid.NewGuid().ToString();

        return await _automation.RetrieveEobsAsync(
            payer,
            request,
            runId);
    }

    public async Task<AppealRunResult> SubmitAppealAsync(AppealRequest request)
    {
        var payer = await _repository.GetAsync(
            request.PayerId,
            request.TaxId);

        var runId = Guid.NewGuid().ToString();

        return await _automation.SubmitAppealAsync(
            payer,
            request,
            runId);
    }
}