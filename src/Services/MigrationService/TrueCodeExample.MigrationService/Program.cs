using Serilog;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Logging;
using TrueCodeExample.MigrationService;

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddYamlConfiguration();
    builder.AddSerilog();

    builder.Services.AddMigrationService(builder.Configuration);

    var host = builder.Build();

    await using var scope = host.Services.CreateAsyncScope();
    var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrationRunner>();
    await migrator.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Migration service failed");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
