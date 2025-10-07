using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saa3idWeb.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_359_add_locationtype_on_locations_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Location",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Location");
        }
    }
}
