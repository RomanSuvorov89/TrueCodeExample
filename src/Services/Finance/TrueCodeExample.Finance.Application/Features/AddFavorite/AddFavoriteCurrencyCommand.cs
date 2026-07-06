using Mediator;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public sealed record AddFavoriteCurrencyCommand(Guid UserId, string CharCode) : IRequest;
