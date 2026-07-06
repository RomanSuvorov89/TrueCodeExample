using Mediator;
using TrueCodeExample.Finance.Application.Contracts;

namespace TrueCodeExample.Finance.Application.Features.GetRates;

public sealed record GetUserRatesQuery(Guid UserId) : IRequest<IReadOnlyList<CurrencyResponse>>;
