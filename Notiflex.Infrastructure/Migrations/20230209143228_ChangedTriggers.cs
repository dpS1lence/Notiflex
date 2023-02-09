using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notiflex.Infrastructure.Migrations
{
    public partial class ChangedTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalUnit",
                table: "NotiflexTriggers");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "NotiflexTriggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "NotiflexTriggers");

            migrationBuilder.AddColumn<int>(
                name: "IntervalUnit",
                table: "NotiflexTriggers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
