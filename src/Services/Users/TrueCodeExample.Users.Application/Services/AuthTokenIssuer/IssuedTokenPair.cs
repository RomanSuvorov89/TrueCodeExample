using TrueCodeExample.Users.Application.DTO;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.Application.Services.AuthTokenIssuer;

public sealed record IssuedTokenPair(AuthResponse Response, RefreshToken RefreshToken);
