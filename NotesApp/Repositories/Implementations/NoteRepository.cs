using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models.DTO;
using NotesApp.Repositories.Interfaces;

namespace NotesApp.Repositories.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private DbManager _context;
        public NoteRepository(DbManager context)
        {
            _context = context;
        }
        public async Task<List<NoteDTO>> GetNotesAsync(int page, float pageSize = 10f)
        {
            var pageCount = Math.Ceiling(_context.Notes.Count() / pageSize);
            return await _context.Notes
                .Select(note => new NoteDTO() { CreationDate = note.CreationDate, Id = note.Id, Text = note.Text, Title = note.Title })
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();
        }
    }
}
