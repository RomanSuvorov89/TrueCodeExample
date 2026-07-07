using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Users.DataAccess;

namespace TrueCodeExample.MigrationService;

public static class RegisterServices
{
    public static IServiceCollection AddMigrationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersDataAccess(configuration, includeHealthChecks: false);
        services.AddFinanceDataAccess(configuration, includeHealthChecks: false);
        services.AddScoped<DatabaseMigrationRunner>();

        return services;
    }
}
