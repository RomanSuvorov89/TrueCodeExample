using TrueCodeExample.Common.Configuration;
using TrueCodeExample.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddMigrationService(builder.Configuration);

var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrationRunner>();
await migrator.RunAsync();
