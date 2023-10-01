using NotesApp.Models.DbModels;
using NotesApp.Models.DTO;

namespace NotesApp.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<List<NoteDTO>> GetNotesAsync(int page, float pageSize = 10f);
        Task<(bool, string)> SaveNoteAsync(Note note);
    }
}
