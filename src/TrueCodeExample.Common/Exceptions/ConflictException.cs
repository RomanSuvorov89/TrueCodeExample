using System.Net;

namespace TrueCodeExample.Common.Exceptions;

public class ConflictException(string message) : DomainException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
