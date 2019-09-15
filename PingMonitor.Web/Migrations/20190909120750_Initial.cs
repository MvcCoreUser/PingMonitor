using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PingMonitor.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Url = table.Column<string>(nullable: true),
                    CheckExecTime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monitorings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ApiInfoId = table.Column<int>(nullable: false),
                    ExecStart = table.Column<DateTime>(nullable: true),
                    ExecTimeInSeconds = table.Column<long>(nullable: true),
                    Accessible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitorings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monitorings_ApiInfos_ApiInfoId",
                        column: x => x.ApiInfoId,
                        principalTable: "ApiInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Monitorings_ApiInfoId",
                table: "Monitorings",
                column: "ApiInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Monitorings");

            migrationBuilder.DropTable(
                name: "ApiInfos");
        }
    }
}
