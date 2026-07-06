using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.RemoveFavorite;

public interface IFavoriteCurrencyRemover
{
    Task<FavoriteCurrency?> GetAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken);

    void Remove(FavoriteCurrency favorite);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
