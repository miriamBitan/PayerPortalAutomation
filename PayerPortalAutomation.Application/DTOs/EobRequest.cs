namespace PayerPortalAutomation.Application.DTOs;

public class EobRequest
{
    public string PayerId { get; init; }

    public string TaxId { get; init; }

    public DateTime DateFrom { get; init; }

    public DateTime DateTo { get; init; }

    public int MaxEobs { get; init; }

    public bool Download { get; init; }

    public bool ForceRedownload { get; init; }
    public Dedupe Dedupe { get; init; }
}
public class Dedupe
{
    public string? Mode { get; init; }
    public string? Scope { get; init; }
}