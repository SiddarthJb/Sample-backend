using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class IntrestsAndLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Interests_InterestsId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Languages_LanguagesId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_InterestsId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_LanguagesId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "InterestsId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "LanguagesId",
                table: "Profiles");

            migrationBuilder.CreateTable(
                name: "InterestsProfile",
                columns: table => new
                {
                    InterestsId = table.Column<int>(type: "int", nullable: false),
                    ProfilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestsProfile", x => new { x.InterestsId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_InterestsProfile_Interests_InterestsId",
                        column: x => x.InterestsId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestsProfile_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguagesProfile",
                columns: table => new
                {
                    LanguagesId = table.Column<int>(type: "int", nullable: false),
                    ProfilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguagesProfile", x => new { x.LanguagesId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_LanguagesProfile_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguagesProfile_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestsProfile_ProfilesId",
                table: "InterestsProfile",
                column: "ProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesProfile_ProfilesId",
                table: "LanguagesProfile",
                column: "ProfilesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestsProfile");

            migrationBuilder.DropTable(
                name: "LanguagesProfile");

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InterestsId",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LanguagesId",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_InterestsId",
                table: "Profiles",
                column: "InterestsId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_LanguagesId",
                table: "Profiles",
                column: "LanguagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Interests_InterestsId",
                table: "Profiles",
                column: "InterestsId",
                principalTable: "Interests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Languages_LanguagesId",
                table: "Profiles",
                column: "LanguagesId",
                principalTable: "Languages",
                principalColumn: "Id");
        }
    }
}
