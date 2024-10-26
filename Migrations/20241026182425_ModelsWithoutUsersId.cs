using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cis_api_legacy_integration_phase_2.Migrations
{
    /// <inheritdoc />
    public partial class ModelsWithoutUsersId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "votes");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "ideas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "votes",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "topics",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ideas",
                newName: "UsersId");
        }
    }
}
