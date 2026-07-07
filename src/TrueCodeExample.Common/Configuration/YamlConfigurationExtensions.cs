using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TrueCodeExample.Common.Configuration;

public static class YamlConfigurationExtensions
{
    public static WebApplicationBuilder AddYamlConfiguration(this WebApplicationBuilder builder)
    {
        ConfigureYaml(builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.EnvironmentName);
        return builder;
    }

    public static HostApplicationBuilder AddYamlConfiguration(this HostApplicationBuilder builder)
    {
        ConfigureYaml(builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.EnvironmentName);
        return builder;
    }

    private static void ConfigureYaml(IConfigurationBuilder configuration, string contentRoot, string environment)
    {
        configuration.Sources.Clear();

        configuration
            .SetBasePath(contentRoot)
            .AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true)
            .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}
