using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Management.Core.Migrations
{
    public partial class theredMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TableNumber",
                table: "Orders",
                newName: "TableId");

            migrationBuilder.AddColumn<string>(
                name: "TableNumber",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "Orders",
                newName: "TableNumber");
        }
    }
}
