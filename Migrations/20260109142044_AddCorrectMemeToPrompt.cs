using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemeMatch.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectMemeToPrompt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectMemeId",
                table: "Prompts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prompts_CorrectMemeId",
                table: "Prompts",
                column: "CorrectMemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prompts_Memes_CorrectMemeId",
                table: "Prompts",
                column: "CorrectMemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prompts_Memes_CorrectMemeId",
                table: "Prompts");

            migrationBuilder.DropIndex(
                name: "IX_Prompts_CorrectMemeId",
                table: "Prompts");

            migrationBuilder.DropColumn(
                name: "CorrectMemeId",
                table: "Prompts");
        }
    }
}
