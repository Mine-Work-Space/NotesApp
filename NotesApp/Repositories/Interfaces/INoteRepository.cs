using NotesApp.Models;
using NotesApp.Models.UI;

namespace NotesApp.Repositories.Interfaces
{
	public interface INoteRepository
	{
		/// <summary>
		///		Last updated count of notes. Can not be set
		/// </summary>
		int NoteCount { get; }
		/// <summary>
		///		Notes with <paramref name="pageSize"/> amount and <paramref name="page"/> page 
		/// </summary>
		/// <param name="page">
		///		notes page to download. <c>must be >= 1</c>
		/// </param>
		/// <param name="pageSize">
		///		count of notes on one page
		/// </param>
		/// <returns>
		///		<see cref="NotesList"/> with notes, total pages, current page and enum <see cref="DisplayType"/>
		/// </returns>
		Task<NotesList> GetNotesByPageAsync(int pages, float pageSize = 5f);
		/// <summary>
		///		Notes in which the searched word was found
		/// </summary>
		/// <param name="searchTerm">
		///		search word or phrase
		/// </param>
		/// <returns>
		///		<see cref="NotesList"/> with notes, total pages, current page and enum <see cref="DisplayType"/>
		/// </returns>
		Task<NotesList> GetNotesBySearchTermAsync(string searchTerm);
		/// <summary>
		///		Notes in which the searched word was found
		/// </summary>
		/// <param name="searchTerm">
		///		search word or phrase
		/// </param>
		/// <returns>
		///		<see cref="Tuple{Boolean, String}"/> Item1 = if note saved successfully, Item2 = message from repository
		/// </returns>
		Task<(bool, string)> SaveNoteAsync(Note note);
		/// <summary>
		///		Update existing note
		/// </summary>
		/// <param name="updatedNote">
		///		updated model
		/// </param>
		/// <returns>
		///		<see cref="Boolean"/> true if note updated successfully, false if not
		/// </returns>
		Task<bool> UpdateNoteAsync(Note updatedNote);
	}
}
