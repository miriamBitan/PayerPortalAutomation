public class EobRunResult : RunResultBase
{
    public DateTime DateFrom { get; init; }
    public DateTime DateTo { get; init; }

    public int EobsFound { get; init; }
    public int Downloaded { get; init; }
    public int SkippedByDedupe { get; init; }

    public List<EobRecord> Items { get; init; } = new();
}