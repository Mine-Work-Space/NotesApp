namespace NotesApp.Models.UI
{
	public class NoteUI
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
		public DateOnly CreationDate { get; set; }
		/// <summary>
		///		Displaying text if length of main <c>Text</c> > 150
		/// </summary>
		public string TextMinimized => Text.Length > 150 ? new string(Text.Take(150).ToArray()) : string.Empty;
		/// <summary>
		///		Buffer of <c>Text</c> if user decide updating selected note
		/// </summary>
		public string EditableText { get; set; } = string.Empty;
		/// <summary>
		///		 True if the note is editing, False if not
		/// </summary>
		public bool IsEditing { get; set; } = false;
		/// <summary>
		///		 Method to set values to view modal (#fullTextModal) easier
		/// </summary>
		public void Deconstruct(out string title, out string fullText)
		{
			title = Title;
			fullText = Text;
		}
	}
	public class NotesList
	{
		public List<NoteUI>? Notes { get; set; }
		/// <summary>
		///		Total amount of pages. Calculate as (int)Math.Ceiling(NoteCount / pageSize);
		/// </summary>
		public int Pages { get; set; }
		/// <summary>
		///		The current page was loaded
		/// </summary>
		public int CurrentPage { get; set; }
		public DisplayType DisplayType { get; set; }
	}
	public enum DisplayType 
	{
		/// <summary>
		///		Displaying notes without pagination navbar
		/// </summary>
		SearchingByText,
		/// <summary>
		///		Displaying notes with pagination navbar
		/// </summary>
		Default 
	}
}
