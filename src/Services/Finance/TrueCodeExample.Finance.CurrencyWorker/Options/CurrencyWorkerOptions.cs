namespace TrueCodeExample.Finance.CurrencyWorker.Options;

public sealed class CurrencyWorkerOptions
{
    public const string SectionName = "CurrencyWorker";

    public int SyncIntervalMinutes { get; set; } = 60;
}
