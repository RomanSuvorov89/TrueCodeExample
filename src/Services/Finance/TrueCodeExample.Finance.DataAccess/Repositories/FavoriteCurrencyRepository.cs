using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.Application.Abstractions;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Repositories;

public sealed class FavoriteCurrencyRepository(FinanceDbContext dbContext) : IFavoriteCurrencyRepository
{
    public async Task<IReadOnlyList<FavoriteCurrency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await dbContext.FavoriteCurrencies.Where(x => x.UserId == userId).ToListAsync(cancellationToken);

    public Task<FavoriteCurrency?> GetAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken)
        => dbContext.FavoriteCurrencies.SingleOrDefaultAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);

    public Task<bool> ExistsAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken)
        => dbContext.FavoriteCurrencies.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);

    public async Task AddAsync(FavoriteCurrency favorite, CancellationToken cancellationToken)
        => await dbContext.FavoriteCurrencies.AddAsync(favorite, cancellationToken);

    public void Remove(FavoriteCurrency favorite)
        => dbContext.FavoriteCurrencies.Remove(favorite);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
