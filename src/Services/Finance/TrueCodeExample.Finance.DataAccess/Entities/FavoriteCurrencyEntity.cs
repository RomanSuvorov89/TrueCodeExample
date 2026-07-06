namespace TrueCodeExample.Finance.DataAccess.Entities;

public class FavoriteCurrencyEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CurrencyId { get; set; }
}
