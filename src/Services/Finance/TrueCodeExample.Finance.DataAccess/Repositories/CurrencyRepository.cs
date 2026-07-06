using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.Application.Abstractions;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Repositories;

public sealed class CurrencyRepository(FinanceDbContext dbContext) : ICurrencyRepository
{
    public async Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken cancellationToken)
        => await dbContext.Currencies.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Currency>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
        => await dbContext.Currencies.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public Task<Currency?> GetByCharCodeAsync(string charCode, CancellationToken cancellationToken)
        => dbContext.Currencies.SingleOrDefaultAsync(x => x.CharCode == charCode, cancellationToken);

    public async Task AddAsync(Currency currency, CancellationToken cancellationToken)
        => await dbContext.Currencies.AddAsync(currency, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
