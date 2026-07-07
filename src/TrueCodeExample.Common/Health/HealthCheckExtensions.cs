using Microsoft.AspNetCore.Builder;

namespace TrueCodeExample.Common.Health;

public static class HealthCheckExtensions
{
    public static WebApplication MapHealthChecksEndpoint(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        return app;
    }
}
