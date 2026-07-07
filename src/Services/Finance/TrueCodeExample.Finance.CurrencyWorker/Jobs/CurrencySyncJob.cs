using Mediator;
using Quartz;
using TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;
using TrueCodeExample.Finance.Infrastructure.Cbr;

namespace TrueCodeExample.Finance.CurrencyWorker.Jobs;

[DisallowConcurrentExecution]
public sealed class CurrencySyncJob(
    IServiceScopeFactory scopeFactory,
    ICbrCurrencyProvider cbrCurrencyProvider,
    ILogger<CurrencySyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        try
        {
            var currencies = await cbrCurrencyProvider.GetDailyCurrenciesAsync(cancellationToken);

            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new UpsertCurrenciesCommand(currencies), cancellationToken);

            logger.LogInformation("Synced {Count} currencies from CBR", currencies.Count);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Failed to sync currencies from CBR");
        }
    }
}
