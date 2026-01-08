using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonProf.Migrations
{
    /// <inheritdoc />
    public partial class refacto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teachers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Teachers",
                type: "text",
                nullable: true);
        }
    }
}
