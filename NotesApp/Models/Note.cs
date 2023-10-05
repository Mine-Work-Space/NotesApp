using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
	// Guid can be replaced with simple int if db won't be merged
	public class Note
	{
		public Guid Id { get; set; }
		[Required(ErrorMessage = "{0} is required")]
		[MinLength(5, ErrorMessage = "{0} must have at least 5 characters")]
		[MaxLength(20, ErrorMessage = "{0} can not be > 50 characters")]
		public string Title { get; set; } = string.Empty;
		[Required(ErrorMessage = "{0} is required")]
		[DataType(DataType.MultilineText)]
		[MaxLength(8192, ErrorMessage = "There is so much text")]
		[MinLength(5, ErrorMessage = "{0} must have at least 5 characters")]
		public string Text { get; set; } = string.Empty;
		[DataType(DataType.Date)]
		public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
		public NpgsqlTsVector? SearchVector { get; set; }
	}
}
