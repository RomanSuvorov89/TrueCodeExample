using Mediator;
using TrueCodeExample.Finance.Application.Features.GetCurrencies;

namespace TrueCodeExample.Finance.Application.Features.GetRates;

public sealed record GetUserRatesQuery(Guid UserId) : IRequest<IReadOnlyList<CurrencyResponse>>;
