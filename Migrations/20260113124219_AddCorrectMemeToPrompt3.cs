using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemeMatch.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectMemeToPrompt3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Memes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Memes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
