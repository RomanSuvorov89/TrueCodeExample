using System.Net;

namespace TrueCodeExample.Common.Exceptions;

public class NotFoundException(string message) : DomainException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
