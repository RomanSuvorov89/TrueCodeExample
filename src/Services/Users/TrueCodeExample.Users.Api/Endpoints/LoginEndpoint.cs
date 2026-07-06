using Mediator;
using TrueCodeExample.Users.Api.Contracts;
using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Application.Features.Login;

namespace TrueCodeExample.Users.Api.Endpoints;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLogin(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new LoginCommand(request.Name, request.Password), cancellationToken);
                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("Login")
            .Produces<AuthResponse>();

        return app;
    }
}
