using System.Net;

namespace TrueCodeExample.Common.Exceptions;

public class UnauthorizedDomainException() : DomainException("Token does not contain a valid user identifier.")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
