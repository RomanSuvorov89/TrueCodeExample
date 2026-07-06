using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCodeExample.Finance.Domain.Entities;

namespace TrueCodeExample.Finance.DataAccess.Configurations;

public sealed class FavoriteCurrencyConfiguration : IEntityTypeConfiguration<FavoriteCurrency>
{
    public void Configure(EntityTypeBuilder<FavoriteCurrency> builder)
    {
        builder.ToTable("favorite_currencies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CurrencyId)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.CurrencyId })
            .IsUnique();

        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
