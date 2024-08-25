using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class subscribed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Subscribed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subscribed",
                table: "AspNetUsers");
        }
    }
}
