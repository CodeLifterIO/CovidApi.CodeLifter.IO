using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    RecordsProcessed = table.Column<int>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Confirmed = table.Column<int>(nullable: true),
                    Deaths = table.Column<int>(nullable: true),
                    Recovered = table.Column<int>(nullable: true),
                    Active = table.Column<int>(nullable: true),
                    IncidenceRate = table.Column<double>(nullable: true),
                    CaseFatalityRatio = table.Column<double>(nullable: true),
                    SourceFile = table.Column<string>(nullable: true),
                    CombinedKey = table.Column<string>(nullable: true),
                    CountrySlugId = table.Column<string>(nullable: true),
                    ProvinceSlugId = table.Column<string>(nullable: true),
                    DistrictSlugId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    RecordsProcessed = table.Column<int>(nullable: true),
                    StartFileName = table.Column<string>(nullable: true),
                    LastCompletedFileName = table.Column<string>(nullable: true),
                    Completed = table.Column<bool>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoCoordinates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCoordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Totals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: true),
                    Confirmed = table.Column<int>(nullable: true),
                    Deaths = table.Column<int>(nullable: true),
                    Active = table.Column<int>(nullable: true),
                    Recovered = table.Column<int>(nullable: true),
                    SourceFile = table.Column<string>(nullable: true),
                    CountrySlugId = table.Column<string>(nullable: true),
                    ProvinceSlugId = table.Column<string>(nullable: true),
                    DistrictSlugId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    SlugId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.UniqueConstraint("AK_Countries_SlugId", x => x.SlugId);
                    table.ForeignKey(
                        name: "FK_Countries_GeoCoordinates_GeoCoordinateId",
                        column: x => x.GeoCoordinateId,
                        principalTable: "GeoCoordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    SlugId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FIPS = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true),
                    ProvinceSlugId = table.Column<string>(nullable: true),
                    CountrySlugId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.UniqueConstraint("AK_Districts_SlugId", x => x.SlugId);
                    table.ForeignKey(
                        name: "FK_Districts_GeoCoordinates_GeoCoordinateId",
                        column: x => x.GeoCoordinateId,
                        principalTable: "GeoCoordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    SlugId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CountrySlugId = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.UniqueConstraint("AK_Provinces_SlugId", x => x.SlugId);
                    table.ForeignKey(
                        name: "FK_Provinces_GeoCoordinates_GeoCoordinateId",
                        column: x => x.GeoCoordinateId,
                        principalTable: "GeoCoordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_GeoCoordinateId",
                table: "Countries",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_SourceFile_CountrySlugId_ProvinceSlugId_DistrictSlugId",
                table: "DataPoints",
                columns: new[] { "SourceFile", "CountrySlugId", "ProvinceSlugId", "DistrictSlugId" },
                unique: true,
                filter: "[SourceFile] IS NOT NULL AND [CountrySlugId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DataUpdates_Id",
                table: "DataUpdates",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_GeoCoordinateId",
                table: "Districts",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoCoordinates_Id",
                table: "GeoCoordinates",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_GeoCoordinateId",
                table: "Provinces",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_Totals_SourceFile_CountrySlugId_ProvinceSlugId_DistrictSlugId",
                table: "Totals",
                columns: new[] { "SourceFile", "CountrySlugId", "ProvinceSlugId", "DistrictSlugId" },
                unique: true,
                filter: "[SourceFile] IS NOT NULL AND [CountrySlugId] IS NOT NULL AND [ProvinceSlugId] IS NOT NULL AND [DistrictSlugId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DataFiles");

            migrationBuilder.DropTable(
                name: "DataPoints");

            migrationBuilder.DropTable(
                name: "DataUpdates");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Totals");

            migrationBuilder.DropTable(
                name: "GeoCoordinates");
        }
    }
}
