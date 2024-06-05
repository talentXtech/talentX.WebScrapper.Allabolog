using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talentX.WebScrapper.Allabolog.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailedScrapOutputData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialScrapOutputData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailedScrapOutputData");

            migrationBuilder.DropTable(
                name: "InitialScrapOutputData");
        }
    }
}
