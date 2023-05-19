using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActuarialApplications.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskFreeRateData",
                columns: table => new
                {
                    ProjectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Maturity = table.Column<int>(type: "INTEGER", nullable: false),
                    SpotValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFreeRateData", x => new { x.ProjectionId, x.Maturity });
                });

            migrationBuilder.CreateTable(
                name: "RiskFreeRates",
                columns: table => new
                {
                    ProjectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ValueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestParameters = table.Column<string>(type: "TEXT", nullable: false),
                    VerifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFreeRates", x => x.ProjectionId);
                });

            migrationBuilder.CreateTable(
                name: "Swap",
                columns: table => new
                {
                    ValueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Tenor = table.Column<int>(type: "INTEGER", nullable: false),
                    SettlementFreq = table.Column<double>(type: "REAL", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Swap", x => new { x.ValueDate, x.Currency, x.Tenor });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskFreeRateData");

            migrationBuilder.DropTable(
                name: "RiskFreeRates");

            migrationBuilder.DropTable(
                name: "Swap");
        }
    }
}
