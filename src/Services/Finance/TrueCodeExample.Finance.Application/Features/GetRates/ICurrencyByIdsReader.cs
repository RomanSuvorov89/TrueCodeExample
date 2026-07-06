using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.GetRates;

public interface ICurrencyByIdsReader
{
    Task<IReadOnlyList<Currency>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
}
