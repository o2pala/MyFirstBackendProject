using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customers_Customerid1",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Customerid1",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Customerid1",
                table: "Contacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Customerid1",
                table: "Contacts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Customerid1",
                table: "Contacts",
                column: "Customerid1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customers_Customerid1",
                table: "Contacts",
                column: "Customerid1",
                principalTable: "Customers",
                principalColumn: "id");
        }
    }
}
