using NotesApp.Models;

namespace NotesApp.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<NotesList> GetNotesAsync(int page, float pageSize = 5f);
        Task<NotesList> GetNotesBySearchTermAsync(string searchTerm);
        Task<(bool, string)> SaveNoteAsync(Note note);
        int NoteCount { get; }
    }
}
