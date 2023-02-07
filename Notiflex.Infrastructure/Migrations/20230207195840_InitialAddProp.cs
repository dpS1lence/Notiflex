using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notiflex.Infrastructure.Migrations
{
    public partial class InitialAddProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserApproved",
                table: "AspNetUsers");
        }
    }
}
