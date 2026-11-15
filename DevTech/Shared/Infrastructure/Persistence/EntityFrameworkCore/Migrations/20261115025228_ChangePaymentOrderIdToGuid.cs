using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangePaymentOrderIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing payments reference orders by number, which is not a valid order GUID
            migrationBuilder.Sql("DELETE FROM payments;");

            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "payments",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Order GUIDs cannot be stored as numbers
            migrationBuilder.Sql("DELETE FROM payments;");

            migrationBuilder.AlterColumn<int>(
                name: "order_id",
                table: "payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");
        }
    }
}
