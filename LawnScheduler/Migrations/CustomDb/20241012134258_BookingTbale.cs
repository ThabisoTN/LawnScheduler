using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawnScheduler.Migrations.CustomDb
{
    /// <inheritdoc />
    public partial class BookingTbale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    MachineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OperatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.MachineId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsConflict = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__73951AED755C7B60", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "MachineId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MachineId",
                table: "Bookings",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_OperatorId",
                table: "Machines",
                column: "OperatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Machines");
        }
    }
}
