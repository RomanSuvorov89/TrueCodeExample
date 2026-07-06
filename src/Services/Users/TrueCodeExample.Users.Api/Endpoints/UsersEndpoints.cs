using System.Security.Claims;
using Mediator;
using TrueCodeExample.Common.Security;
using TrueCodeExample.Users.Api.Features.Login;
using TrueCodeExample.Users.Api.Features.Refresh;
using TrueCodeExample.Users.Api.Features.Register;
using TrueCodeExample.Users.Application.Features.Login;
using TrueCodeExample.Users.Application.Features.Logout;
using TrueCodeExample.Users.Application.Features.Refresh;
using TrueCodeExample.Users.Application.Features.Register;
using TrueCodeExample.Users.Application.DTO;

namespace TrueCodeExample.Users.Api.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RegisterUserCommand(request.Name, request.Password), cancellationToken);
                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("Register")
            .Produces<AuthResponse>();

        group.MapPost("/login", async (LoginRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new LoginCommand(request.Name, request.Password), cancellationToken);
                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("Login")
            .Produces<AuthResponse>();

        group.MapPost("/refresh", async (RefreshRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RefreshTokenCommand(request.RefreshToken), cancellationToken);
                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("Refresh")
            .Produces<AuthResponse>();

        group.MapPost("/logout", async (ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new LogoutCommand(principal.GetUserId()), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("Logout");

        return app;
    }
}
