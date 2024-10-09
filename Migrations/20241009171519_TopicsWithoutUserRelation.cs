using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cis_api_legacy_integration_phase_2.Migrations
{
    /// <inheritdoc />
    public partial class TopicsWithoutUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_topics_users_UsersId",
                table: "topics");

            migrationBuilder.DropIndex(
                name: "IX_topics_UsersId",
                table: "topics");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "topics",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OwnerLogin",
                table: "topics",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "topics",
                type: "varchar(36)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_topics_UserId",
                table: "topics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_topics_users_UserId",
                table: "topics",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_topics_users_UserId",
                table: "topics");

            migrationBuilder.DropIndex(
                name: "IX_topics_UserId",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "OwnerLogin",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "topics");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "topics",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_topics_UsersId",
                table: "topics",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_topics_users_UsersId",
                table: "topics",
                column: "UsersId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
