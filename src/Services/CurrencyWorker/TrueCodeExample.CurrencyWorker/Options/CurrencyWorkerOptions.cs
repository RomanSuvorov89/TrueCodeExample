namespace TrueCodeExample.CurrencyWorker.Options;

public sealed class CurrencyWorkerOptions
{
    public const string SectionName = "CurrencyWorker";

    public int SyncIntervalMinutes { get; init; } = 60;

    public string CbrBaseUrl { get; init; } = "http://www.cbr.ru";
}
