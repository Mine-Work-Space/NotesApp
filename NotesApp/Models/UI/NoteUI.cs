using Microsoft.AspNetCore.Mvc.RazorPages;
using NpgsqlTypes;
using System.Xml.Linq;

namespace NotesApp.Models.UI
{
	public class NoteUI
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
		public DateOnly CreationDate { get; set; }
		public string TextMinimized => Text.Length > 150 ? new string(Text.Take(150).ToArray()) : string.Empty;
		public string EditableText { get; set; } = string.Empty;
		public bool IsEditing { get; set; } = false;
		public void GetDetailsData(out string title, out string fullText)
		{
			title = Title;
			fullText = Text;
		}
	}
	public class NotesList
	{
		public List<NoteUI>? Notes { get; set; }
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
		public DisplayType DisplayType { get; set; }
	}
	public enum DisplayType { SearchingByText, Default }
}
