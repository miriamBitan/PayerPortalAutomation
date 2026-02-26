public abstract class RunResultBase
{
    public string RunId { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string PayerId { get; init; } = default!;
    public string TaxId { get; init; } = default!;
}