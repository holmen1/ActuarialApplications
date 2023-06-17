using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActuarialApplications.Migrations.LocalLifeDb
{
    public partial class InitialCreateLife : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashFlows",
                columns: table => new
                {
                    ValueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ContractNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Benefit = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashFlows", x => new { x.ValueDate, x.ContractNo, x.Month });
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractNo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ValueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sex = table.Column<string>(type: "TEXT", nullable: false),
                    VestingAge = table.Column<int>(type: "INTEGER", nullable: false),
                    GuaranteeBenefit = table.Column<double>(type: "REAL", nullable: false),
                    PayPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    Table = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashFlows");

            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
