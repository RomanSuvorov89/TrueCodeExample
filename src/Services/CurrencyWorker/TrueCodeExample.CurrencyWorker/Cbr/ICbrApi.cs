using Refit;

namespace TrueCodeExample.CurrencyWorker.Cbr;

public interface ICbrApi
{
    [Get("/scripts/XML_daily.asp")]
    Task<HttpResponseMessage> GetDailyCurrenciesAsync(CancellationToken cancellationToken = default);
}
