using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCodeExample.Finance.DataAccess.Entities;

namespace TrueCodeExample.Finance.DataAccess.Configurations;

public sealed class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyEntity>
{
    public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
    {
        builder.ToTable("currencies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CharCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasIndex(x => x.CharCode)
            .IsUnique();

        builder.Property(x => x.NumCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Nominal)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("numeric(18,4)");

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();
    }
}
