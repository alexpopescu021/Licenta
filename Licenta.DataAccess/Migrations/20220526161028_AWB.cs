using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.DataAccess.Migrations
{
    public partial class AWB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AWB",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AWB",
                table: "Orders");
        }
    }
}
