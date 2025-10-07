using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saa3idWeb.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_733_make_createdat_and_deletedat_not_required_location_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hotline",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Hotline",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hotline");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Hotline");
        }
    }
}
