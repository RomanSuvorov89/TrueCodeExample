using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrueCodeExample.Finance.DataAccess.Migrations;

[DbContext(typeof(FinanceDbContext))]
[Migration("20260101000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "currencies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CharCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                NumCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Nominal = table.Column<int>(type: "integer", nullable: false),
                Value = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_currencies", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "favorite_currencies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                CurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_favorite_currencies", x => x.Id);
                table.ForeignKey(
                    name: "FK_favorite_currencies_currencies_CurrencyId",
                    column: x => x.CurrencyId,
                    principalTable: "currencies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_currencies_CharCode",
            table: "currencies",
            column: "CharCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_favorite_currencies_CurrencyId",
            table: "favorite_currencies",
            column: "CurrencyId");

        migrationBuilder.CreateIndex(
            name: "IX_favorite_currencies_UserId_CurrencyId",
            table: "favorite_currencies",
            columns: new[] { "UserId", "CurrencyId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "favorite_currencies");

        migrationBuilder.DropTable(name: "currencies");
    }
}
