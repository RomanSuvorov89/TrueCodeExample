using Mediator;
using TrueCodeExample.Users.Api.Contracts;
using TrueCodeExample.Users.Application.Contracts;
using TrueCodeExample.Users.Application.Features.Register;

namespace TrueCodeExample.Users.Api.Endpoints;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegister(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new RegisterUserCommand(request.Name, request.Password), cancellationToken);
                return Results.Ok(response);
            })
            .AllowAnonymous()
            .WithName("Register")
            .Produces<AuthResponse>();

        return app;
    }
}
