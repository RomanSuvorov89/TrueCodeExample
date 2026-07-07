using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TrueCodeExample.Common.Logging;

public static class SerilogExtensions
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            ConfigureLogger(loggerConfiguration, context.Configuration, services, context.HostingEnvironment.ApplicationName));

        return builder;
    }

    public static HostApplicationBuilder AddSerilog(this HostApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, loggerConfiguration) =>
            ConfigureLogger(loggerConfiguration, builder.Configuration, services, builder.Environment.ApplicationName));

        return builder;
    }

    public static WebApplication UseSerilogRequestLogging(this WebApplication app)
    {
        SerilogApplicationBuilderExtensions.UseSerilogRequestLogging(app);
        return app;
    }

    private static void ConfigureLogger(
        LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        IServiceProvider services,
        string applicationName)
    {
        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName);

        var seqUrl = configuration["Seq:ServerUrl"];
        if (!string.IsNullOrWhiteSpace(seqUrl))
        {
            loggerConfiguration.WriteTo.Seq(seqUrl);
        }
    }
}
