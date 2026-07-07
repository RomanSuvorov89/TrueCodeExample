using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TrueCodeExample.Finance.CurrencyWorker.Options;

namespace TrueCodeExample.Finance.CurrencyWorker.Health;

public sealed class CurrencySyncHealthCheck(
    ICurrencySyncState syncState,
    IOptions<CurrencyWorkerOptions> options) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var lastSync = syncState.LastSuccessfulSyncUtc;
        if (lastSync is null)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Currency sync has not completed yet."));
        }

        var maxAge = TimeSpan.FromMinutes(options.Value.SyncIntervalMinutes * 2 + 15);
        if (DateTime.UtcNow - lastSync.Value > maxAge)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                $"Last currency sync was at {lastSync:O}, which exceeds the allowed age of {maxAge}."));
        }

        return Task.FromResult(HealthCheckResult.Healthy(
            $"Last sync at {lastSync:O}, {syncState.LastSyncedCount} currencies."));
    }
}
