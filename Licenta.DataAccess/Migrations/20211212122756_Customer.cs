using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Licenta.DataAccess.Migrations
{
    public partial class Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Adresses",
                table: "Adresses");

            migrationBuilder.RenameTable(
                name: "Adresses",
                newName: "LocationAddresses");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "LocationAddresses",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationAddresses",
                table: "LocationAddresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PhoneNo = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ContactDetailsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Contact_ContactDetailsId",
                        column: x => x.ContactDetailsId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationAddresses_CustomerId",
                table: "LocationAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactDetailsId",
                table: "Customers",
                column: "ContactDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationAddresses_Customers_CustomerId",
                table: "LocationAddresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationAddresses_Customers_CustomerId",
                table: "LocationAddresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationAddresses",
                table: "LocationAddresses");

            migrationBuilder.DropIndex(
                name: "IX_LocationAddresses_CustomerId",
                table: "LocationAddresses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "LocationAddresses");

            migrationBuilder.RenameTable(
                name: "LocationAddresses",
                newName: "Adresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adresses",
                table: "Adresses",
                column: "Id");
        }
    }
}
