using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Middleware;
using TrueCodeExample.Common.Swagger;
using TrueCodeExample.Users.Api.Endpoints;
using TrueCodeExample.Users.Application;
using TrueCodeExample.Users.Application.Abstractions;
using TrueCodeExample.Users.DataAccess;
using TrueCodeExample.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddSwaggerWithBearer("TrueCode Users API");
builder.Services.AddUsersApplication();
builder.Services.AddUsersInfrastructure();
builder.Services.AddUsersDataAccess(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration, options =>
{
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var jti = context.Principal?.FindFirstValue("jti");
            if (string.IsNullOrEmpty(jti))
            {
                return;
            }

            var store = context.HttpContext.RequestServices.GetRequiredService<ITokenRevocationStore>();
            if (await store.IsRevokedAsync(jti, context.HttpContext.RequestAborted))
            {
                context.Fail("Token has been revoked.");
            }
        }
    };
});

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
