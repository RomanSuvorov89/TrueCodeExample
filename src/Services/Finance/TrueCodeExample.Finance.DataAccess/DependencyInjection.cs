using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Finance.Application.Abstractions;
using TrueCodeExample.Finance.DataAccess.Repositories;

namespace TrueCodeExample.Finance.DataAccess;

public static class DependencyInjection
{
    private const string ConnectionStringName = "FinanceDb";

    public static IServiceCollection AddFinanceDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is missing.");

        services.AddDbContext<FinanceDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IFavoriteCurrencyRepository, FavoriteCurrencyRepository>();

        return services;
    }
}
