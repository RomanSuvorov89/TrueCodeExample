using Mediator;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;

public sealed class UpsertCurrenciesCommandHandler(ICurrencyUpsertStore currencies)
    : IRequestHandler<UpsertCurrenciesCommand>
{
    public async ValueTask<Unit> Handle(UpsertCurrenciesCommand request, CancellationToken cancellationToken)
    {
        if (request.Currencies.Count == 0) return Unit.Value;

        var existing = (await currencies.GetAllAsync(cancellationToken))
            .ToDictionary(c => c.CharCode, StringComparer.OrdinalIgnoreCase);

        foreach (var data in request.Currencies)
        {
            if (existing.TryGetValue(data.CharCode, out var currency))
            {
                currency.Update(data.NumCode, data.Name, data.Nominal, data.Value, data.UpdatedAtUtc);
                await currencies.UpdateAsync(currency, cancellationToken);
            }
            else
            {
                await currencies.AddAsync(
                    new Currency(data.CharCode, data.NumCode, data.Name, data.Nominal, data.Value, data.UpdatedAtUtc),
                    cancellationToken);
            }
        }

        await currencies.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
