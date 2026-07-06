using Microsoft.EntityFrameworkCore;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
    public DbSet<Currency> Currencies => Set<Currency>();

    public DbSet<FavoriteCurrency> FavoriteCurrencies => Set<FavoriteCurrency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);
    }
}
