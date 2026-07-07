namespace TrueCodeExample.Finance.Infrastructure.Options;

public sealed class CbrOptions
{
    public const string SectionName = "Cbr";

    public string BaseUrl { get; set; } = "http://www.cbr.ru";
}
