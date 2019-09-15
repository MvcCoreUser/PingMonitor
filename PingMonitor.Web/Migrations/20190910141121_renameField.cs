using Microsoft.EntityFrameworkCore.Migrations;

namespace PingMonitor.Web.Migrations
{
    public partial class renameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExecTimeInSeconds",
                table: "Monitorings",
                newName: "ExecTimeInMilliseconds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExecTimeInMilliseconds",
                table: "Monitorings",
                newName: "ExecTimeInSeconds");
        }
    }
}
