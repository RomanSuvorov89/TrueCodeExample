using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public interface ICurrencyListReader
{
    Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken cancellationToken);
}
