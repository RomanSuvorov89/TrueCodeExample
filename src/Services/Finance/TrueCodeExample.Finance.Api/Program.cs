using Serilog;
using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Health;
using TrueCodeExample.Common.Logging;
using TrueCodeExample.Common.Middleware;
using TrueCodeExample.Common.Swagger;
using TrueCodeExample.Finance.Api.Endpoints;
using TrueCodeExample.Finance.Application;
using TrueCodeExample.Finance.DataAccess;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddYamlConfiguration();
    builder.UseSerilog();

    builder.Services.AddSwaggerWithBearer("TrueCode Finance API");
    builder.Services.AddFinanceApplication();
    builder.Services.AddFinanceDataAccess(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseTrueCodeExceptionHandling();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecksEndpoint();
    app.MapFinanceEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Finance API terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
