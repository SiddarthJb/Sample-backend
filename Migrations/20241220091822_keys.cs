using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subscribed",
                table: "AspNetUsers",
                newName: "IsSubscribed");

            migrationBuilder.AddColumn<int>(
                name: "Keys",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keys",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsSubscribed",
                table: "AspNetUsers",
                newName: "Subscribed");
        }
    }
}
