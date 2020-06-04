using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddCurrentSummaryDataToLocales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaseFatalityRatio",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Confirmed",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deaths",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recovered",
                table: "Provinces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaseFatalityRatio",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Confirmed",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deaths",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recovered",
                table: "Districts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaseFatalityRatio",
                table: "DataPoints",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "DataPoints",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaseFatalityRatio",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Confirmed",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deaths",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncidenceRate",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recovered",
                table: "Countries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "CaseFatalityRatio",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Deaths",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Recovered",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "CaseFatalityRatio",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "Deaths",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "Recovered",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "CaseFatalityRatio",
                table: "DataPoints");

            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "DataPoints");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CaseFatalityRatio",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Deaths",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IncidenceRate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Recovered",
                table: "Countries");
        }
    }
}
