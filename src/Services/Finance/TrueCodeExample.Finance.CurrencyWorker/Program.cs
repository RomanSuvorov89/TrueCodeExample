using System.Text;
using Serilog;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Logging;
using TrueCodeExample.Finance.Application;
using TrueCodeExample.Finance.CurrencyWorker;
using TrueCodeExample.Finance.DataAccess;
using TrueCodeExample.Finance.Infrastructure;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.AddYamlConfiguration();
    builder.AddSerilog();

    builder.Services.AddFinanceApplication();
    builder.Services.AddFinanceDataAccess(builder.Configuration);
    builder.Services.AddFinanceInfrastructure(builder.Configuration);
    builder.Services.AddCurrencyWorker(builder.Configuration);

    var host = builder.Build();
    await host.RunAsync();
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
