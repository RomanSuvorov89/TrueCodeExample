using System.Net;

namespace TrueCodeExample.Common.Exceptions;

public class ValidationException : DomainException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors) : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
}
