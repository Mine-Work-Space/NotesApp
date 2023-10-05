using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace NotesApp.Migrations
{
	/// <inheritdoc />
	public partial class InitMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Notes",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
					Text = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: false),
					CreationDate = table.Column<DateOnly>(type: "date", nullable: false),
					SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
						.Annotation("Npgsql:TsVectorConfig", "english")
						.Annotation("Npgsql:TsVectorProperties", new[] { "Title", "Text" })
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Notes", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Notes_SearchVector",
				table: "Notes",
				column: "SearchVector")
				.Annotation("Npgsql:IndexMethod", "GIN");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Notes");
		}
	}
}
