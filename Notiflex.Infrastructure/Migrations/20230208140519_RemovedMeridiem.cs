using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notiflex.Infrastructure.Migrations
{
    public partial class RemovedMeridiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meridiem",
                table: "NotiflexTriggers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meridiem",
                table: "NotiflexTriggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
