using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    public partial class updatedInitialScrapDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchFieldText",
                table: "InitialScrapOutputData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchFieldText",
                table: "InitialScrapOutputData");
        }
    }
}
