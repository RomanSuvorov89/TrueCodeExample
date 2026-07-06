using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TrueCodeExample.Common.Configuration;

public static class YamlConfigurationExtensions
{
    public static WebApplicationBuilder AddTrueCodeYamlConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();

        var contentRoot = builder.Environment.ContentRootPath;
        var environment = builder.Environment.EnvironmentName;

        builder.Configuration
            .SetBasePath(contentRoot)
            .AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true)
            .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
