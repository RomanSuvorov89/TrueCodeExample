using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using TrueCodeExample.CurrencyWorker.Cbr;
using TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

namespace TrueCodeExample.CurrencyWorker.Jobs;

[DisallowConcurrentExecution]
public sealed class CurrencySyncJob(
    IServiceScopeFactory scopeFactory,
    ICbrApi cbrApi,
    ILogger<CurrencySyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        try
        {
            var response = await cbrApi.GetDailyCurrenciesAsync(cancellationToken);
            var valCurs = await CbrCurrencyMapper.ParseAsync(response, cancellationToken);
            var currencies = CbrCurrencyMapper.ToCurrencyData(valCurs);

            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new UpsertCurrenciesCommand(currencies), cancellationToken);

            logger.LogInformation("Synced {Count} currencies from CBR (date {Date})", currencies.Count, valCurs.Date);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Failed to sync currencies from CBR");
        }
    }
}
