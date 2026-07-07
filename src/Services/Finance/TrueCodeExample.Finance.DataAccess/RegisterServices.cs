using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;
using TrueCodeExample.Finance.Application.Features.GetRates;
using TrueCodeExample.Finance.Application.Features.RemoveFavorite;
using TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;
using TrueCodeExample.Finance.DataAccess.Repositories;

namespace TrueCodeExample.Finance.DataAccess;

public static class RegisterServices
{
    private const string ConnectionStringName = "FinanceDb";

    public static IServiceCollection AddFinanceDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is missing.");

        services.AddDbContext<FinanceDbContext>(options => options.UseNpgsql(connectionString));

        services.AddHealthChecks()
            .AddDbContextCheck<FinanceDbContext>();

        services.AddScoped<CurrencyRepository>();
        services.AddScoped<ICurrencyUpsertStore>(sp => sp.GetRequiredService<CurrencyRepository>());
        services.AddScoped<ICurrencyListReader>(sp => sp.GetRequiredService<CurrencyRepository>());
        services.AddScoped<ICurrencyByIdsReader>(sp => sp.GetRequiredService<CurrencyRepository>());
        services.AddScoped<ICurrencyByCharCodeReader>(sp => sp.GetRequiredService<CurrencyRepository>());

        services.AddScoped<FavoriteCurrencyRepository>();
        services.AddScoped<IFavoriteCurrencyByUserReader>(sp => sp.GetRequiredService<FavoriteCurrencyRepository>());
        services.AddScoped<IFavoriteCurrencyWriter>(sp => sp.GetRequiredService<FavoriteCurrencyRepository>());
        services.AddScoped<IFavoriteCurrencyRemover>(sp => sp.GetRequiredService<FavoriteCurrencyRepository>());

        return services;
    }
}
