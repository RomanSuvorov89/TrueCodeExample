using Mediator;

namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public sealed record GetCurrenciesQuery : IRequest<IReadOnlyList<CurrencyResponse>>;
