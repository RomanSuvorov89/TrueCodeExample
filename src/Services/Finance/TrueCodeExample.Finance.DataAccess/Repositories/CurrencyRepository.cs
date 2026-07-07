using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;
using TrueCodeExample.Finance.Application.Features.GetRates;
using TrueCodeExample.Finance.Application.Features.UpsertCurrencies;
using TrueCodeExample.Finance.DataAccess.Entities;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Repositories;

public sealed class CurrencyRepository(FinanceDbContext dbContext)
    : ICurrencyUpsertStore, ICurrencyListReader, ICurrencyByIdsReader, ICurrencyByCharCodeReader
{
    public async Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await dbContext.Currencies.AsNoTracking().ToListAsync(cancellationToken);
        return entities.Select(x => x.ToDomain()).ToList();
    }

    public async Task<IReadOnlyList<Currency>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var entities = await dbContext.Currencies.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        return entities.Select(x => x.ToDomain()).ToList();
    }

    public async Task<Currency?> GetByCharCodeAsync(string charCode, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Currencies
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.CharCode == charCode, cancellationToken);
        return entity?.ToDomain();
    }

    public async Task AddAsync(Currency currency, CancellationToken cancellationToken)
        => await dbContext.Currencies.AddAsync(currency.ToEntity(), cancellationToken);

    public Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        dbContext.Currencies.Update(currency.ToEntity());
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
