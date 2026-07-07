using System.Security.Claims;
using Mediator;
using TrueCodeExample.Common.Security;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;
using TrueCodeExample.Finance.Application.Features.AddFavorite;
using TrueCodeExample.Finance.Application.Features.GetRates;
using TrueCodeExample.Finance.Application.Features.RemoveFavorite;

namespace TrueCodeExample.Finance.Api.Endpoints;

public static class FinanceEndpoints
{
    public static IEndpointRouteBuilder MapFinanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/finance")
            .WithTags("Finance");

        group.MapGet("/currencies", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var currencies = await sender.Send(new GetCurrenciesQuery(), cancellationToken);
                return Results.Ok(currencies);
            })
            .AllowAnonymous()
            .WithName("GetCurrencies")
            .Produces<IReadOnlyList<CurrencyResponse>>();

        group.MapGet("/rates", async (ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                var rates = await sender.Send(new GetUserRatesQuery(principal.GetUserId()), cancellationToken);
                return Results.Ok(rates);
            })
            .RequireAuthorization()
            .WithName("GetUserRates")
            .Produces<IReadOnlyList<CurrencyResponse>>();

        group.MapPost("/favorites/{charCode}", async (string charCode, ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new AddFavoriteCurrencyCommand(principal.GetUserId(), charCode), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("AddFavorite");

        group.MapDelete("/favorites/{charCode}", async (string charCode, ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new RemoveFavoriteCurrencyCommand(principal.GetUserId(), charCode), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("RemoveFavorite");

        return app;
    }
}
