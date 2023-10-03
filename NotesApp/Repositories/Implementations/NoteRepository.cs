using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models.DbModels;
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
        public async Task<NotesList> GetNotesAsync(int page, float pageSize = 10f)
        {
            var query = _context.Set<Note>();

            var pageCount = Math.Ceiling(_context.Notes.Count() / pageSize);
            var notes = await _context.Notes
                .OrderBy(n => n.CreationDate)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();
            return new NotesList()
            {
                Notes = notes.ToList(),
                CurrentPage = page,
                Pages = (int)pageCount
            };
        }

        public async Task<(bool, string)> SaveNoteAsync(Note note)
        {
            try
            {
                note.CreationDate = DateOnly.FromDateTime(DateTime.Now);
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();
                return (true, "New note was successfully added!");
            }
            catch(UniqueConstraintException ex)
            {
                return (false, $"{ex.Message}. Try change title.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
