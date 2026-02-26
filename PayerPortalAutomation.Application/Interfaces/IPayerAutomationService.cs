using PayerPortalAutomation.Application.DTOs;

namespace PayerPortalAutomation.Application.Interfaces;

public interface IPayerAutomationService
{
    Task<EobRunResult> RetrieveEobsAsync(EobRequest request);

    Task<AppealRunResult> SubmitAppealAsync(AppealRequest request);
}