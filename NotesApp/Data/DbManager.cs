using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NotesApp.Models;
using System.Reflection.Metadata;
using System.Text;

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
					"english",  // Text search config
					n => new { n.Title, n.Text})  // Included properties
					.HasIndex(p => p.SearchVector)
					.HasMethod("GIN"); // Index method on the search vector (GIN or GIST)
			#region DataSeeding
			//for (int i = 0; i < 100; i++)
			//{
			//	modelBuilder.Entity<Note>().HasData(new Note()
			//	{
			//		Id = Guid.NewGuid(),
			//		CreationDate = DateOnly.FromDateTime(DateTime.Now),
			//		Text = LoremIpsum(5, 30, 5, 40, 1),
			//		Title = $"Title-{i}"
			//	});
			//}
			#endregion
		}
		private string LoremIpsum(int minWords, int maxWords,
			int minSentences, int maxSentences,
			int numParagraphs)
		{

			var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
		"adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
		"tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

			var rand = new Random();
			int numSentences = rand.Next(maxSentences - minSentences)
				+ minSentences + 1;
			int numWords = rand.Next(maxWords - minWords) + minWords + 1;

			StringBuilder result = new StringBuilder();

			for (int p = 0; p < numParagraphs; p++)
			{
				for (int s = 0; s < numSentences; s++)
				{
					for (int w = 0; w < numWords; w++)
					{
						if (w > 0) { result.Append(" "); }
						result.Append(words[rand.Next(words.Length)]);
					}
					result.Append(". ");
				}
			}

			return result.ToString();
		}

	}
}
