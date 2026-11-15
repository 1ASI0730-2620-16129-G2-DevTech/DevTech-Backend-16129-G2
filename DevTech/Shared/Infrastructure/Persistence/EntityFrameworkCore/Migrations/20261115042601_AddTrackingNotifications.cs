using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_trackings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    current_stage = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    estimated_delivery = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_order_trackings", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tracking_stage_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    order_tracking_id = table.Column<int>(type: "int", nullable: false),
                    stage = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_tracking_stage_records", x => x.id);
                    table.ForeignKey(
                        name: "f_k_tracking_stage_records_order_trackings_order_tracking_id",
                        column: x => x.order_tracking_id,
                        principalTable: "order_trackings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ux_order_trackings_order_id",
                table: "order_trackings",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_tracking_stage_records_order_tracking_id",
                table: "tracking_stage_records",
                column: "order_tracking_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tracking_stage_records");

            migrationBuilder.DropTable(
                name: "order_trackings");
        }
    }
}
