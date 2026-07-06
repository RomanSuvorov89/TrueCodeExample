using Mediator;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;

namespace TrueCodeExample.Finance.Application.Features.GetRates;

public sealed class GetUserRatesQueryHandler(
    IFavoriteCurrencyByUserReader favorites,
    ICurrencyByIdsReader currencies)
    : IRequestHandler<GetUserRatesQuery, IReadOnlyList<CurrencyResponse>>
{
    public async ValueTask<IReadOnlyList<CurrencyResponse>> Handle(GetUserRatesQuery request, CancellationToken cancellationToken)
    {
        var userFavorites = await favorites.GetByUserAsync(request.UserId, cancellationToken);
        if (userFavorites.Count == 0) return [];

        var currencyIds = userFavorites.Select(f => f.CurrencyId).ToArray();
        var favoriteCurrencies = await currencies.GetByIdsAsync(currencyIds, cancellationToken);

        return favoriteCurrencies
            .OrderBy(c => c.CharCode)
            .Select(c => new CurrencyResponse(c.CharCode, c.Name, c.Nominal, c.Value, c.UpdatedAtUtc))
            .ToList();
    }
}
