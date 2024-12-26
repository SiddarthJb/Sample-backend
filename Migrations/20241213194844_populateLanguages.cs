using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z1.Migrations
{
    /// <inheritdoc />
    public partial class populateLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Insert default record into the Languages table
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 1, "Assamese", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 2, "Bengali", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 3, "Bhojpuri", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 4, "English", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 5, "Gujrati", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 6, "Hindi", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 7, "Kanada", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 8, "Malayalam", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 9, "Marathi", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 10, "Odia", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 11, "Punjabi", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 12, "Tamil", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 13, "Telugu", 1, DateTime.Now, true }
            );
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Language", "CreatedBy", "CreatedAt", "IsActive" },
                values: new object[] { 14, "Urdu", 1, DateTime.Now, true }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
