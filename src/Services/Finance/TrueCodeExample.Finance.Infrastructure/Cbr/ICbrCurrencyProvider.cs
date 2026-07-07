using TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;

namespace TrueCodeExample.Finance.Infrastructure.Cbr;

public interface ICbrCurrencyProvider
{
    Task<IReadOnlyCollection<CurrencyData>> GetDailyCurrenciesAsync(CancellationToken cancellationToken);
}
