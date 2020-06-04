using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class RemoveIncidenceRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "CaseFatalityRatio",
                table: "Countries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "Provinces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "Districts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaseFatalityRatio",
                table: "Countries",
                type: "float",
                nullable: true);
        }
    }
}
