using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notiflex.Infrastructure.Migrations
{
    public partial class TriggerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "NotiflexTriggers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Meridiem",
                table: "NotiflexTriggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Minutes",
                table: "NotiflexTriggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "NotiflexTriggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "NotiflexTriggers");

            migrationBuilder.DropColumn(
                name: "Meridiem",
                table: "NotiflexTriggers");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "NotiflexTriggers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "NotiflexTriggers");
        }
    }
}
