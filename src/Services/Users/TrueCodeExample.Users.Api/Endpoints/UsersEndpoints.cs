namespace TrueCodeExample.Users.Api.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapRegister();
        group.MapLogin();
        group.MapLogout();

        return app;
    }
}
