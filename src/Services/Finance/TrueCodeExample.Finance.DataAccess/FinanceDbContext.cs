using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.DataAccess.Entities;

namespace TrueCodeExample.Finance.DataAccess;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();

    public DbSet<FavoriteCurrencyEntity> FavoriteCurrencies => Set<FavoriteCurrencyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);
    }
}
