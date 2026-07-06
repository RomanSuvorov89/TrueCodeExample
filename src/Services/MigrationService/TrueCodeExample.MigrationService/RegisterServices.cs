using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Users.DataAccess;

namespace TrueCodeExample.MigrationService;

public static class RegisterServices
{
    public static IServiceCollection AddMigrationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersDataAccess(configuration);
        services.AddFinanceDataAccess(configuration);
        services.AddScoped<DatabaseMigrationRunner>();

        return services;
    }
}
