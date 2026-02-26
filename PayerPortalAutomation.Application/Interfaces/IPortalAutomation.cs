using PayerPortalAutomation.Application.DTOs;
using PayerPortalAutomation.Domain.Entities;

namespace PayerPortalAutomation.Application.Interfaces;

public interface IPortalAutomation
{
    Task<EobRunResult> RetrieveEobsAsync(
        PayerTarget payer,
        EobRequest request,
        string runId);

    Task<AppealRunResult> SubmitAppealAsync(
        PayerTarget payer,
        AppealRequest request,
        string runId);
}