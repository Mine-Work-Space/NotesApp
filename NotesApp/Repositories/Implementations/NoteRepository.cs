using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using NotesApp.Data;
using NotesApp.Models;
using NotesApp.Repositories.Interfaces;

namespace NotesApp.Repositories.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private DbManager _context;
        public int NoteCount { get;  private set; }
        public NoteRepository(DbManager context)
        {
            _context = context;
		}
        public async Task<NotesList> GetNotesAsync(int page, float pageSize = 5f)
        {
            var pageCount = GetPageCount(pageSize);
			var notes = await _context.Notes
                .OrderBy(n => n.CreationDate)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();
            return new NotesList()
            {
                Notes = notes,
                CurrentPage = page,
                Pages = pageCount,
                DisplayType = DisplayType.Default
			};
        }

		public async Task<NotesList> GetNotesBySearchTermAsync(string searchTerm)
		{
			var notes = await _context.Notes
	            .Where(p => p.SearchVector.Matches(searchTerm))
	            .ToListAsync();

			return new NotesList()
			{
				Notes = notes,
				CurrentPage = 1,
				Pages = 0,
				DisplayType = DisplayType.SearchingByText
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
        private int GetPageCount(float pageSize)
        {
            NoteCount = _context.Notes.Count();
			return (int)Math.Ceiling(NoteCount / pageSize);
        }
    }
}
