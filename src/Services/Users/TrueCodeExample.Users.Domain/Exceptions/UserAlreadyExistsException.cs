using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Users.Domain.Exceptions;

public sealed class UserAlreadyExistsException(string name) : ConflictException($"User with name '{name}' already exists.");
