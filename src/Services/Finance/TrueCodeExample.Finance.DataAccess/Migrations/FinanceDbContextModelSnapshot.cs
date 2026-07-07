using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace TrueCodeExample.Finance.DataAccess.Migrations;

[DbContext(typeof(FinanceDbContext))]
partial class FinanceDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("TrueCodeExample.Finance.DataAccess.Entities.CurrencyEntity", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<string>("CharCode")
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnType("character varying(3)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<int>("Nominal")
                .HasColumnType("integer");

            b.Property<string>("NumCode")
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnType("character varying(3)");

            b.Property<DateTime>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<decimal>("Value")
                .HasColumnType("numeric(18,4)");

            b.HasKey("Id");

            b.HasIndex("CharCode")
                .IsUnique();

            b.ToTable("currencies");
        });

        modelBuilder.Entity("TrueCodeExample.Finance.DataAccess.Entities.FavoriteCurrencyEntity", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<Guid>("CurrencyId")
                .HasColumnType("uuid");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("CurrencyId");

            b.HasIndex("UserId", "CurrencyId")
                .IsUnique();

            b.ToTable("favorite_currencies");
        });

        modelBuilder.Entity("TrueCodeExample.Finance.DataAccess.Entities.FavoriteCurrencyEntity", b =>
        {
            b.HasOne("TrueCodeExample.Finance.DataAccess.Entities.CurrencyEntity", null)
                .WithMany()
                .HasForeignKey("CurrencyId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}
