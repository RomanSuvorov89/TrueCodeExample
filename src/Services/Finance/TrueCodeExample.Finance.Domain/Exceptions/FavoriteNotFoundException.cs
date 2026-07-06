using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Finance.Domain.Exceptions;

public sealed class FavoriteNotFoundException(string charCode) : NotFoundException($"Currency '{charCode}' is not in favorites.");
