using System;
using CodeLifter.Covid19.Data.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeLifter.Covid19.Data.Migrations
{
    public partial class AddAdminToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminTokens_Token",
                table: "AdminTokens",
                column: "Token",
                unique: true);


            string guidSource = Environment.GetEnvironmentVariable("ADMIN_AUTH_TOKEN");
            if (!string.IsNullOrWhiteSpace(guidSource))
            {
                Guid adminToken = new Guid(guidSource);
                AdminToken token = new AdminToken()
                {
                    Token = adminToken,
                };
                AdminToken.Insert(token);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminTokens");
        }
    }
}
