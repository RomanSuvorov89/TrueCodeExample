using Serilog;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Logging;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddYamlConfiguration();
    builder.UseSerilog();

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.MapReverseProxy();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Gateway terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
