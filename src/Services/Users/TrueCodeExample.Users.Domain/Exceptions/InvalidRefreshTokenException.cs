using System.Net;
using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Users.Domain.Exceptions;

public sealed class InvalidRefreshTokenException() : DomainException("Invalid or expired refresh token")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
