using Refit;

namespace TrueCodeExample.Finance.Infrastructure.Cbr;

public interface ICbrApi
{
    [Get("/scripts/XML_daily.asp")]
    Task<HttpResponseMessage> GetDailyCurrenciesAsync(CancellationToken cancellationToken = default);
}
