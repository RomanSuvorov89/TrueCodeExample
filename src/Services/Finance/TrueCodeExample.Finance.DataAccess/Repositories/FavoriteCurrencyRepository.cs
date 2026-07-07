using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Application.Features.GetRates;
using TrueCodeExample.Finance.Application.Features.RemoveFavorite;
using TrueCodeExample.Finance.DataAccess.Entities;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Repositories;

public sealed class FavoriteCurrencyRepository(FinanceDbContext dbContext)
    : IFavoriteCurrencyByUserReader, IFavoriteCurrencyWriter, IFavoriteCurrencyRemover
{
    public async Task<IReadOnlyList<FavoriteCurrency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await dbContext.FavoriteCurrencies
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        return entities.Select(x => x.ToDomain()).ToList();
    }

    public async Task<FavoriteCurrency?> GetAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.FavoriteCurrencies
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);
        return entity?.ToDomain();
    }

    public Task<bool> ExistsAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken)
        => dbContext.FavoriteCurrencies.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);

    public async Task AddAsync(FavoriteCurrency favorite, CancellationToken cancellationToken)
        => await dbContext.FavoriteCurrencies.AddAsync(favorite.ToEntity(), cancellationToken);

    public void Remove(FavoriteCurrency favorite)
        => dbContext.FavoriteCurrencies.Remove(favorite.ToEntity());

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
