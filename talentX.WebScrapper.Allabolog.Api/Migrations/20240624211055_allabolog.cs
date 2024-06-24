using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    public partial class allabolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "allabolog");

            migrationBuilder.CreateTable(
                name: "DetailedScrapOutputData",
                schema: "allabolog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchFieldText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verksamhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfSearch = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllabolagUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrgNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfEstablishment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revenue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNames = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailedScrapOutputData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitialScrapOutputData",
                schema: "allabolog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verksamhet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFieldText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialScrapOutputData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailedScrapOutputData",
                schema: "allabolog");

            migrationBuilder.DropTable(
                name: "InitialScrapOutputData",
                schema: "allabolog");
        }
    }
}
