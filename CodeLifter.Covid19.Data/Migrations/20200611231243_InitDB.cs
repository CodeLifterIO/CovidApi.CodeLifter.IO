using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataCollectionStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordsProcessed = table.Column<int>(nullable: false),
                    LastRunCompleted = table.Column<DateTime>(nullable: false),
                    LastRunStarted = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCollectionStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoCoordinates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCoordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_GeoCoordinates_GeoCoordinateId",
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
                    CountryId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Provinces_GeoCoordinates_GeoCoordinateId",
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
                    FIPS = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    GeoCoordinateId = table.Column<int>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_GeoCoordinates_GeoCoordinateId",
                        column: x => x.GeoCoordinateId,
                        principalTable: "GeoCoordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Confirmed = table.Column<int>(nullable: true),
                    Deaths = table.Column<int>(nullable: true),
                    Recovered = table.Column<int>(nullable: true),
                    Active = table.Column<int>(nullable: true),
                    IncidenceRate = table.Column<double>(nullable: true),
                    CaseFatalityRatio = table.Column<double>(nullable: true),
                    SourceFile = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataPoints_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataPoints_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataPoints_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Totals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    Confirmed = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false),
                    Active = table.Column<int>(nullable: false),
                    Recovered = table.Column<int>(nullable: false),
                    SourceFile = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Totals_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Totals_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Totals_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_GeoCoordinateId",
                table: "Countries",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DataCollectionStatistics_Id",
                table: "DataCollectionStatistics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_CountryId",
                table: "DataPoints",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_DistrictId",
                table: "DataPoints",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_ProvinceId",
                table: "DataPoints",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_LastUpdate_CountryId_ProvinceId_DistrictId",
                table: "DataPoints",
                columns: new[] { "LastUpdate", "CountryId", "ProvinceId", "DistrictId" },
                unique: true,
                filter: "[LastUpdate] IS NOT NULL AND [CountryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountryId",
                table: "Districts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_GeoCoordinateId",
                table: "Districts",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_FIPS_Name",
                table: "Districts",
                columns: new[] { "FIPS", "Name" },
                unique: true,
                filter: "[FIPS] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GeoCoordinates_Latitude_Longitude",
                table: "GeoCoordinates",
                columns: new[] { "Latitude", "Longitude" },
                unique: true,
                filter: "[Latitude] IS NOT NULL AND [Longitude] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CountryId",
                table: "Provinces",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_GeoCoordinateId",
                table: "Provinces",
                column: "GeoCoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Name",
                table: "Provinces",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Totals_CountryId",
                table: "Totals",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Totals_DistrictId",
                table: "Totals",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Totals_ProvinceId",
                table: "Totals",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Totals_SourceFile_CountryId_ProvinceId_DistrictId",
                table: "Totals",
                columns: new[] { "SourceFile", "CountryId", "ProvinceId", "DistrictId" },
                unique: true,
                filter: "[SourceFile] IS NOT NULL AND [CountryId] IS NOT NULL AND [ProvinceId] IS NOT NULL AND [DistrictId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataCollectionStatistics");

            migrationBuilder.DropTable(
                name: "DataPoints");

            migrationBuilder.DropTable(
                name: "Totals");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "GeoCoordinates");
        }
    }
}
