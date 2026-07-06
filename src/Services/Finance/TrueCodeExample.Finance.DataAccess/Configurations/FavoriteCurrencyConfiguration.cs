using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCodeExample.Finance.DataAccess.Entities;

namespace TrueCodeExample.Finance.DataAccess.Configurations;

public sealed class FavoriteCurrencyConfiguration : IEntityTypeConfiguration<FavoriteCurrencyEntity>
{
    public void Configure(EntityTypeBuilder<FavoriteCurrencyEntity> builder)
    {
        builder.ToTable("favorite_currencies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CurrencyId)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.CurrencyId })
            .IsUnique();

        builder.HasOne<CurrencyEntity>()
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
