using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.DataAccess.Migrations
{
    public partial class update_StreetNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "LocationAddresses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StreetNumber",
                table: "LocationAddresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
