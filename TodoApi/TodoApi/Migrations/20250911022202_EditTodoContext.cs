using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class EditTodoContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customers_CustomerId",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Contacts",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_CustomerId",
                table: "Contacts",
                newName: "IX_Contacts_customerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customers_customerId",
                table: "Contacts",
                column: "customerId",
                principalTable: "Customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customers_customerId",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Contacts",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_customerId",
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
    }
}
