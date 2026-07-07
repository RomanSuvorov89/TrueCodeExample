using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using TrueCodeExample.Finance.Infrastructure.Cbr;
using TrueCodeExample.Finance.Infrastructure.Options;

namespace TrueCodeExample.Finance.Infrastructure;

public static class RegisterServices
{
    public static IServiceCollection AddFinanceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CbrOptions>(configuration.GetSection(CbrOptions.SectionName));

        services.AddRefitClient<ICbrApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CbrOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });

        services.AddSingleton<ICbrCurrencyProvider, CbrCurrencyProvider>();

        return services;
    }
}
