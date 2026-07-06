using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Middleware;
using TrueCodeExample.Common.Swagger;
using TrueCodeExample.Users.Api.Endpoints;
using TrueCodeExample.Users.Application;
using TrueCodeExample.Users.DataAccess;
using TrueCodeExample.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddSwaggerWithBearer("TrueCode Users API");
builder.Services.AddUsersApplication();
builder.Services.AddUsersInfrastructure();
builder.Services.AddUsersDataAccess(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTrueCodeExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapUsersEndpoints();

app.Run();
