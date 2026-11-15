using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialLaundryOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "laundry_resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    laundry_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    assigned_order_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    version = table.Column<Guid>(type: "char(36)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_laundry_resources", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "washing_cycles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    compatible_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_washing_cycles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "laundry_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    order_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    laundry_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    current_stage = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    priority = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    washing_cycle_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    resource_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    expected_completion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    version = table.Column<Guid>(type: "char(36)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_laundry_orders", x => x.id);
                    table.ForeignKey(
                        name: "f_k_laundry_orders_laundry_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "laundry_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_laundry_orders_washing_cycles_washing_cycle_id",
                        column: x => x.washing_cycle_id,
                        principalTable: "washing_cycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_orders_laundry_id",
                table: "laundry_orders",
                column: "laundry_id");

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_orders_order_id",
                table: "laundry_orders",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_orders_resource_id",
                table: "laundry_orders",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_orders_washing_cycle_id",
                table: "laundry_orders",
                column: "washing_cycle_id");

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_resources_assigned_order_id",
                table: "laundry_resources",
                column: "assigned_order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_laundry_resources_laundry_id",
                table: "laundry_resources",
                column: "laundry_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "laundry_orders");

            migrationBuilder.DropTable(
                name: "laundry_resources");

            migrationBuilder.DropTable(
                name: "washing_cycles");
        }
    }
}
