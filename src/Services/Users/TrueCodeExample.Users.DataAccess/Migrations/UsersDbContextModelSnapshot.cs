using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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

        modelBuilder.Entity("TrueCodeExample.Users.Domain.Entities.RevokedToken", b =>
        {
            b.Property<string>("Jti")
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.Property<DateTime>("ExpiresAtUtc")
                .HasColumnType("timestamp with time zone");

            b.HasKey("Jti");

            b.ToTable("revoked_tokens");
        });

        modelBuilder.Entity("TrueCodeExample.Users.Domain.Entities.User", b =>
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
    }
}
