using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Users.DataAccess;

namespace TrueCodeExample.MigrationService;

public sealed class DatabaseMigrationRunner(
    UsersDbContext usersDbContext,
    FinanceDbContext financeDbContext,
    ILogger<DatabaseMigrationRunner> logger)
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Applying Users database migrations...");
        await usersDbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Users database migrations applied");

        logger.LogInformation("Applying Finance database migrations...");
        await financeDbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Finance database migrations applied");
    }
}
