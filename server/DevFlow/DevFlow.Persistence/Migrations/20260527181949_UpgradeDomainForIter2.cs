using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDomainForIter2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TimeLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "TimeLogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Developers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeLogs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Developers");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "TimeLogs",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
