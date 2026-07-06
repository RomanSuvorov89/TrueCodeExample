using TrueCodeExample.Common.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddTrueCodeYamlConfiguration();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();
