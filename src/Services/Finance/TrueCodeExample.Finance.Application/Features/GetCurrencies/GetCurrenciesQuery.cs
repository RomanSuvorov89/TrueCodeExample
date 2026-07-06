using Mediator;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;

namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public sealed record GetCurrenciesQuery : IRequest<IReadOnlyList<CurrencyResponse>>;
