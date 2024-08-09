using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class PopulateLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert default record into the Languages table
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 1, "Assamese", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 2, "Bengali", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 3, "Bhojpuri", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 4, "English", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 5, "Gujrati", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 6, "Hindi", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 7, "Kanada", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 8, "Malayalam", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 9, "Marathi", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 10, "Odia", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 11, "Punjabi", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 12, "Tamil", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 13, "Telugu", DateTime.Now, DateTime.Now }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "ModifiedAt", "CreatedAt" },
                values: new object[] { 14, "Urdu", DateTime.Now, DateTime.Now }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
