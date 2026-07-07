using TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;

namespace TrueCodeExample.Finance.Infrastructure.Cbr;

public sealed class CbrCurrencyProvider(ICbrApi cbrApi) : ICbrCurrencyProvider
{
    public async Task<IReadOnlyCollection<CurrencyData>> GetDailyCurrenciesAsync(CancellationToken cancellationToken)
    {
        var response = await cbrApi.GetDailyCurrenciesAsync(cancellationToken);
        var valCurs = await CbrCurrencyMapper.ParseAsync(response, cancellationToken);
        return CbrCurrencyMapper.ToCurrencyData(valCurs);
    }
}
