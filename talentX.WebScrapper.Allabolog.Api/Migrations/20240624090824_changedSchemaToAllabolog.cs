using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    public partial class changedSchemaToAllabolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Allabolog");

            migrationBuilder.RenameTable(
                name: "InitialScrapOutputData",
                newName: "InitialScrapOutputData",
                newSchema: "Allabolog");

            migrationBuilder.RenameTable(
                name: "DetailedScrapOutputData",
                newName: "DetailedScrapOutputData",
                newSchema: "Allabolog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "InitialScrapOutputData",
                schema: "Allabolog",
                newName: "InitialScrapOutputData");

            migrationBuilder.RenameTable(
                name: "DetailedScrapOutputData",
                schema: "Allabolog",
                newName: "DetailedScrapOutputData");
        }
    }
}
