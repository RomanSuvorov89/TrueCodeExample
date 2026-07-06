using Mediator;

namespace TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

public sealed record UpsertCurrenciesCommand(IReadOnlyCollection<CurrencyData> Currencies) : IRequest;
