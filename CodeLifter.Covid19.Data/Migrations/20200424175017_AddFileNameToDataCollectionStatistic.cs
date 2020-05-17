using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddFileNameToDataCollectionStatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "DataCollectionStatistics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "DataCollectionStatistics");
        }
    }
}
