using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public interface IFavoriteCurrencyWriter
{
    Task<bool> ExistsAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken);

    Task AddAsync(FavoriteCurrency favorite, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
