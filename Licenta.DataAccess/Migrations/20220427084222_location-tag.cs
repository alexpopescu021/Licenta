using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.DataAccess.Migrations
{
    public partial class locationtag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "LocationAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "LocationAddresses");
        }
    }
}
