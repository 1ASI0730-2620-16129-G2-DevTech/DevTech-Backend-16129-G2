using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "payment_method",
                table: "payments",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                // Payments registered before the payment method existed are taken as card payments.
                defaultValue: "CARD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_method",
                table: "payments");
        }
    }
}
