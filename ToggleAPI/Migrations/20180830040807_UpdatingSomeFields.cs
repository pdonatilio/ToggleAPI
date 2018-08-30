using Microsoft.EntityFrameworkCore.Migrations;

namespace ToggleAPI.Migrations
{
    public partial class UpdatingSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "value",
                table: "Toggles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "value",
                table: "Toggles",
                nullable: false,
                defaultValue: 0);
        }
    }
}
