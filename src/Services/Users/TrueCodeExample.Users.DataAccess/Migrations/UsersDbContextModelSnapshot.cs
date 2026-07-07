using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace TrueCodeExample.Users.DataAccess.Migrations;

[DbContext(typeof(UsersDbContext))]
partial class UsersDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("TrueCodeExample.Users.DataAccess.Entities.RefreshTokenEntity", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTime>("ExpiresAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTime?>("RevokedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("TokenHash")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("TokenHash")
                .IsUnique();

            b.HasIndex("UserId");

            b.ToTable("refresh_tokens");
        });

        modelBuilder.Entity("TrueCodeExample.Users.DataAccess.Entities.UserEntity", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.Property<string>("PasswordHash")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.HasIndex("Name")
                .IsUnique();

            b.ToTable("users");
        });

        modelBuilder.Entity("TrueCodeExample.Users.DataAccess.Entities.RefreshTokenEntity", b =>
        {
            b.HasOne("TrueCodeExample.Users.DataAccess.Entities.UserEntity", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}
