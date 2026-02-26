namespace PayerPortalAutomation.Application.DTOs;

public class AppealRequest
{
    public string PayerId { get; init; }

    public string TaxId { get; init; }

    public string ClaimId { get; init; }

    public string AppealType { get; init; }

    public string Reason { get; init; }

    public bool Submit { get; init; }
    public IEnumerable<AttachmentDto>? Attachments { get; init; }
}