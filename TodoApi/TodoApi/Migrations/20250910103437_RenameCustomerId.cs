using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customers_Customerid",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "Customerid",
                table: "Contacts",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_Customerid",
                table: "Contacts",
                newName: "IX_Contacts_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customers_CustomerId",
                table: "Contacts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customers_CustomerId",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Contacts",
                newName: "Customerid");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_CustomerId",
                table: "Contacts",
                newName: "IX_Contacts_Customerid");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customers_Customerid",
                table: "Contacts",
                column: "Customerid",
                principalTable: "Customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
