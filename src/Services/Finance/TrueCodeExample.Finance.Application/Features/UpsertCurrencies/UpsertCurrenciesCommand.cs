using Mediator;
using TrueCodeExample.Finance.Application.Contracts;

namespace TrueCodeExample.Finance.Application.Features.UpsertCurrencies;

public sealed record UpsertCurrenciesCommand(IReadOnlyCollection<CurrencyData> Currencies) : IRequest;
