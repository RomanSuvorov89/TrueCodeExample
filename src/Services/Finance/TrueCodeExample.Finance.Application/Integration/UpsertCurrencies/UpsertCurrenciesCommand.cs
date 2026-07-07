using Mediator;

namespace TrueCodeExample.Finance.Application.Integration.UpsertCurrencies;

public sealed record UpsertCurrenciesCommand(IReadOnlyCollection<CurrencyData> Currencies) : IRequest;
