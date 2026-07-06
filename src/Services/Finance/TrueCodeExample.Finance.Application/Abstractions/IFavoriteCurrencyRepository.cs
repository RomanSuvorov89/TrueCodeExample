using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Abstractions;

public interface IFavoriteCurrencyRepository
{
    Task<IReadOnlyList<FavoriteCurrency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<FavoriteCurrency?> GetAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken);

    Task AddAsync(FavoriteCurrency favorite, CancellationToken cancellationToken);

    void Remove(FavoriteCurrency favorite);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
