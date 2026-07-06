using TrueCodeExample.Common.Authentication;
using TrueCodeExample.Common.Configuration;
using TrueCodeExample.Common.Middleware;
using TrueCodeExample.Common.Swagger;
using TrueCodeExample.Finance.Api.Endpoints;
using TrueCodeExample.Finance.Application;
using TrueCodeExample.Finance.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddSwaggerWithBearer("TrueCode Finance API");
builder.Services.AddFinanceApplication();
builder.Services.AddFinanceDataAccess(builder.Configuration);
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

app.MapFinanceEndpoints();

app.Run();
