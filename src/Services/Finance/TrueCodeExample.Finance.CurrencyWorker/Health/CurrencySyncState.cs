namespace TrueCodeExample.Finance.CurrencyWorker.Health;

public interface ICurrencySyncState
{
    DateTime? LastSuccessfulSyncUtc { get; }
    int? LastSyncedCount { get; }

    void RecordSuccess(int count, DateTime syncedAtUtc);
}

public sealed class CurrencySyncState : ICurrencySyncState
{
    private DateTime? _lastSuccessfulSyncUtc;
    private int? _lastSyncedCount;

    public DateTime? LastSuccessfulSyncUtc => _lastSuccessfulSyncUtc;
    public int? LastSyncedCount => _lastSyncedCount;

    public void RecordSuccess(int count, DateTime syncedAtUtc)
    {
        _lastSuccessfulSyncUtc = syncedAtUtc;
        _lastSyncedCount = count;
    }
}
