using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace NotesApp.Migrations
{
    /// <inheritdoc />
    public partial class AnotherMethodIndexingTitleTextMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_Title_Text",
                table: "Notes");

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Notes",
                type: "tsvector",
                nullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Title", "Text" });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SearchVector",
                table: "Notes",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_SearchVector",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Notes");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Title_Text",
                table: "Notes",
                columns: new[] { "Title", "Text" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }
    }
}
