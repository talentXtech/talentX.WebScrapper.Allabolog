using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    public partial class updatedverksmahet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "InitialScrapOutputData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Verksamhet",
                table: "InitialScrapOutputData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "DetailedScrapOutputData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Verksamhet",
                table: "DetailedScrapOutputData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "InitialScrapOutputData");

            migrationBuilder.DropColumn(
                name: "Verksamhet",
                table: "InitialScrapOutputData");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "DetailedScrapOutputData");

            migrationBuilder.DropColumn(
                name: "Verksamhet",
                table: "DetailedScrapOutputData");
        }
    }
}
