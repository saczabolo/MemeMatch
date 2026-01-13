using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemeMatch.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectMemeToPrompt4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Memes",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Memes",
                newName: "ImageUrl");
        }
    }
}
