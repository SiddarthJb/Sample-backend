using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMatchLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartialMatches");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Matches",
                newName: "IsPartial");

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Skipped",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "User1Liked",
                table: "Matches",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "User1LikedTime",
                table: "Matches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "User2Liked",
                table: "Matches",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "User2LikedTime",
                table: "Matches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_User1Id",
                table: "Matches",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_User2Id",
                table: "Matches",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_User1Id",
                table: "Matches",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_User2Id",
                table: "Matches",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_User1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_User2Id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_User1Id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_User2Id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Skipped",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "User1Liked",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "User1LikedTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "User2Liked",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "User2LikedTime",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "IsPartial",
                table: "Matches",
                newName: "Status");

            migrationBuilder.AddColumn<bool>(
                name: "Temp",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PartialMatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<int>(type: "int", nullable: false),
                    User2Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skipped = table.Column<int>(type: "int", nullable: false),
                    User1Liked = table.Column<bool>(type: "bit", nullable: true),
                    User1LikedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    User2Liked = table.Column<bool>(type: "bit", nullable: true),
                    User2LikedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartialMatches_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PartialMatches_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartialMatches_User1Id",
                table: "PartialMatches",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_PartialMatches_User2Id",
                table: "PartialMatches",
                column: "User2Id");
        }
    }
}
