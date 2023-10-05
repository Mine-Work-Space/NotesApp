using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NotesApp.Models;

namespace NotesApp.Data
{
	public class DbManager : DbContext
	{
		public DbManager(DbContextOptions<DbManager> options) : base(options) { }
		public DbSet<Note> Notes { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// For getting concrete Exception message, not DbException
			// See details https://github.com/Giorgi/EntityFramework.Exceptions
			optionsBuilder.UseExceptionProcessor();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Note>()
			.HasGeneratedTsVectorColumn(
					p => p.SearchVector,
					"english",                          // Text search config
					n => new { n.Title, n.Text })       // Included properties
					.HasIndex(p => p.SearchVector)
					.HasMethod("GIN");                  // Index method on the search vector (GIN)
		}
	}
}
