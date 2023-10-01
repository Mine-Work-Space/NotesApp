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
        public async Task<List<NoteDTO>> GetNotesAsync(int page, float pageSize = 10f)
        {
            var pageCount = Math.Ceiling(_context.Notes.Count() / pageSize);
            return await _context.Notes
                .Select(note => new NoteDTO() { CreationDate = note.CreationDate, Id = note.Id, Text = note.Text, Title = note.Title })
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();
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
