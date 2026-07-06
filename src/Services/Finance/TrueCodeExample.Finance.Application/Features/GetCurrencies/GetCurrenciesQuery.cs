using Mediator;
using TrueCodeExample.Finance.Application.Contracts;

namespace TrueCodeExample.Finance.Application.Features.GetCurrencies;

public sealed record GetCurrenciesQuery : IRequest<IReadOnlyList<CurrencyResponse>>;
