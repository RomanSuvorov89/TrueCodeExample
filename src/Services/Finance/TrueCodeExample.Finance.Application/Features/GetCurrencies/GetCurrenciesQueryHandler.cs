using Mediator;

namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public sealed class GetCurrenciesQueryHandler(ICurrencyListReader currencies)
    : IRequestHandler<GetCurrenciesQuery, IReadOnlyList<CurrencyResponse>>
{
    public async ValueTask<IReadOnlyList<CurrencyResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var all = await currencies.GetAllAsync(cancellationToken);

        return all
            .OrderBy(c => c.CharCode)
            .Select(c => new CurrencyResponse(c.CharCode, c.Name, c.Nominal, c.Value, c.UpdatedAtUtc))
            .ToList();
    }
}
