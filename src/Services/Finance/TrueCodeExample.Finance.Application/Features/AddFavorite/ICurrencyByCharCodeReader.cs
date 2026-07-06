using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.Application.Features.AddFavorite;

public interface ICurrencyByCharCodeReader
{
    Task<Currency?> GetByCharCodeAsync(string charCode, CancellationToken cancellationToken);
}
