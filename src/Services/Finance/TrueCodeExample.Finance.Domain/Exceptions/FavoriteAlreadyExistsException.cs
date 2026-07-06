using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Finance.Domain.Exceptions;

public sealed class FavoriteAlreadyExistsException(string charCode) : ConflictException($"Currency '{charCode}' is already in favorites.");
