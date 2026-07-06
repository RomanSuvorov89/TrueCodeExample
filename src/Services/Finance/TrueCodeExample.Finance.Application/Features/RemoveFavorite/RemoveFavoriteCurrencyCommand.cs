using Mediator;

namespace TrueCodeExample.Finance.Application.Features.RemoveFavorite;

public sealed record RemoveFavoriteCurrencyCommand(Guid UserId, string CharCode) : IRequest;
