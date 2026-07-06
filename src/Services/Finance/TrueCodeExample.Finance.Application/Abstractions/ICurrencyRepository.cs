using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Abstractions;

public interface ICurrencyRepository
{
    Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<Currency>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);

    Task<Currency?> GetByCharCodeAsync(string charCode, CancellationToken cancellationToken);

    Task AddAsync(Currency currency, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
