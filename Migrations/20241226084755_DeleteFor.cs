using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSeen",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "DeleteFor1",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteFor2",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeenBy1",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeenBy2",
                table: "Messages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteFor1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DeleteFor2",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SeenBy1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SeenBy2",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "isSeen",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
