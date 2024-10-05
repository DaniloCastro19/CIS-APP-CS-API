using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cis_api_legacy_integration_phase_2.Migrations
{
    /// <inheritdoc />
    public partial class IdeaSchemaWithTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ideas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ideas");
        }
    }
}
