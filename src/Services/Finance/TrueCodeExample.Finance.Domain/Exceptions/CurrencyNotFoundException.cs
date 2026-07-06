using TrueCodeExample.Common.Exceptions;

namespace TrueCodeExample.Finance.Domain.Exceptions;

public sealed class CurrencyNotFoundException(string charCode) : NotFoundException($"Currency '{charCode}' was not found.");
