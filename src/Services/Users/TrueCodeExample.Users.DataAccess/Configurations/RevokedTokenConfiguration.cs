using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCodeExample.Users.Domain.Entities;

namespace TrueCodeExample.Users.DataAccess.Configurations;

public sealed class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder.ToTable("revoked_tokens");

        builder.HasKey(x => x.Jti);

        builder.Property(x => x.Jti)
            .HasMaxLength(64);

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();
    }
}
