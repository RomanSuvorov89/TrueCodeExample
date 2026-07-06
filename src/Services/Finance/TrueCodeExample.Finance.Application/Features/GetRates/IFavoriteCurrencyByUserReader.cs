using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.GetRates;

public interface IFavoriteCurrencyByUserReader
{
    Task<IReadOnlyList<FavoriteCurrency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
}
