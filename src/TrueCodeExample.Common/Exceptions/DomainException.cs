using System.Net;

namespace TrueCodeExample.Common.Exceptions;

public abstract class DomainException(string message) : Exception(message)
{
    public abstract HttpStatusCode StatusCode { get; }
}
