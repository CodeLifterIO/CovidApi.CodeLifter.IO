using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddSourceFileToDataPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceFile",
                table: "DataPoints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceFile",
                table: "DataPoints");
        }
    }
}
