namespace TrueCodeExample.Finance.DataAccess.Entities;

public class CurrencyEntity
{
    public Guid Id { get; set; }
    public string CharCode { get; set; } = string.Empty;
    public string NumCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Nominal { get; set; }
    public decimal Value { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
