using System.Net;
using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Users.Domain.Exceptions;

public sealed class InvalidCredentialsException() : DomainException("Invalid username or password")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
