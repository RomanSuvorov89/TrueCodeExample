using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

public interface ICurrencyUpsertStore
{
    Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(Currency currency, CancellationToken cancellationToken);

    Task UpdateAsync(Currency currency, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
