using Microsoft.Extensions.Configuration;
using NetEscapades.Configuration.Yaml;

namespace TrueCodeExample.Api.Configuration;

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
