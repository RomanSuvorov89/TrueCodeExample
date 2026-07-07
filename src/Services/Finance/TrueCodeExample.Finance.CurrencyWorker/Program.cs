using System.Text;
using Serilog;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Health;
using TrueCodeExample.Common.Logging;
using TrueCodeExample.Finance.Application;
using TrueCodeExample.Finance.CurrencyWorker;
using TrueCodeExample.Finance.CurrencyWorker.Health;
using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Finance.Infrastructure;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddYamlConfiguration();
    builder.UseSerilog();

    builder.Services.AddFinanceApplication();
    builder.Services.AddFinanceDataAccess(builder.Configuration, includeHealthChecks: false);
    builder.Services.AddFinanceInfrastructure(builder.Configuration);
    builder.Services.AddCurrencyWorker(builder.Configuration);
    builder.Services.AddSingleton<ICurrencySyncState, CurrencySyncState>();
    builder.Services.AddHealthChecks()
        .AddCheck<CurrencySyncHealthCheck>("currency-sync");

    var app = builder.Build();
    app.MapHealthChecksEndpoint();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Finance currency worker terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
