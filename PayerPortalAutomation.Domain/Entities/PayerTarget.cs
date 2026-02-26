namespace PayerPortalAutomation.Domain.Entities;

public class PayerTarget
{
    public string PayerId { get; init; }

    public string TaxId { get; init; }

    public string PortalUrl { get; init; }

    public string Username { get; init; }

    public string Password { get; init; }

    public int DefaultMinDelayMs { get; init; }

    public int DefaultMaxConcurrency { get; init; }
}