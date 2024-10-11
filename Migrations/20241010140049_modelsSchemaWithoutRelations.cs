using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cis_api_legacy_integration_phase_2.Migrations
{
    /// <inheritdoc />
    public partial class modelsSchemaWithoutRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_votes_ideas_IdeasId",
            //     table: "votes");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_votes_users_UsersId",
            //     table: "votes");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_ideas_topics_TopicsId",
            //     table: "ideas");

            // migrationBuilder.DropIndex(
            //     name: "IX_votes_IdeasId",
            //     table: "votes");

            // migrationBuilder.DropIndex(
            //     name: "IX_votes_UsersId",
            //     table: "votes");

            // migrationBuilder.DropIndex(
            //     name: "IX_ideas_TopicsId",
            //     table: "ideas");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "votes",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "IdeasId",
                table: "votes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.AddColumn<string>(
            //     name: "IdeaName",
            //     table: "votes",
            //     type: "longtext",
            //     nullable: false)
            //     .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.AddColumn<string>(
            //     name: "OwnerLogin",
            //     table: "votes",
            //     type: "longtext",
            //     nullable: false)
            //     .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "ideas",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TopicsId",
                table: "ideas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OwnerLogin",
                table: "ideas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TopicName",
                table: "ideas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdeaName",
                table: "votes");

            migrationBuilder.DropColumn(
                name: "OwnerLogin",
                table: "votes");

            migrationBuilder.DropColumn(
                name: "OwnerLogin",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "TopicName",
                table: "ideas");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "votes",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "IdeasId",
                table: "votes",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "ideas",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TopicsId",
                table: "ideas",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_votes_IdeasId",
                table: "votes",
                column: "IdeasId");

            migrationBuilder.CreateIndex(
                name: "IX_votes_UsersId",
                table: "votes",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_TopicsId",
                table: "ideas",
                column: "TopicsId");
        }
    }
}
