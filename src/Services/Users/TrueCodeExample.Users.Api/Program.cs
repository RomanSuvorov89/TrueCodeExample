using Serilog;
using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Health;
using TrueCodeExample.Common.Logging;
using TrueCodeExample.Common.Middleware;
using TrueCodeExample.Common.Swagger;
using TrueCodeExample.Users.Api.Endpoints;
using TrueCodeExample.Users.Application;
using TrueCodeExample.Users.DataAccess;
using TrueCodeExample.Users.Infrastructure;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddYamlConfiguration();
    builder.UseSerilog();

    builder.Services.AddSwaggerWithBearer("TrueCode Users API");
    builder.Services.AddUsersApplication();
    builder.Services.AddUsersInfrastructure();
    builder.Services.AddUsersDataAccess(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandling();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecksEndpoint();
    app.MapUsersEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Users API terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class UsersApiProgram;
