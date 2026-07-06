using System.Security.Claims;
using Mediator;
using TrueCodeExample.Common.Security;
using TrueCodeExample.Users.Application.Features.Logout;

namespace TrueCodeExample.Users.Api.Endpoints;

public static class LogoutEndpoint
{
    public static IEndpointRouteBuilder MapLogout(this IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", async (ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                var jti = principal.GetJti();
                if (string.IsNullOrEmpty(jti))
                {
                    return Results.NoContent();
                }

                var expiresAtUtc = principal.GetExpiration() ?? DateTime.UtcNow;
                await sender.Send(new LogoutCommand(jti, expiresAtUtc), cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("Logout");

        return app;
    }
}
