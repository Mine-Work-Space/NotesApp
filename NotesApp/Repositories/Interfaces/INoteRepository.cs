using NotesApp.Models.DbModels;
using NotesApp.Models.DTO;

namespace NotesApp.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<NotesList> GetNotesAsync(int page, float pageSize = 10f);
        Task<(bool, string)> SaveNoteAsync(Note note);
    }
}
