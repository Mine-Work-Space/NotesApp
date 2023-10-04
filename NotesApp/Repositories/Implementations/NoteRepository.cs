using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;
using NotesApp.Models.UI;
using NotesApp.Repositories.Interfaces;

namespace NotesApp.Repositories.Implementations
{
	/// <inheritdoc />
	public sealed class NoteRepository : INoteRepository
    {
        private DbManager _context;
        public int NoteCount { get;  private set; }

        public NoteRepository(DbManager context) => _context = context;

        public async Task<NotesList> GetNotesAsync(int page, float pageSize = 5f)
        {
            var pageCount = GetPageCount(pageSize);
			var notes = await _context.Notes
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .Select(n => new NoteUI()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Text = n.Text,
                    CreationDate = n.CreationDate
                })
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
            //var s = EF.Functions.PlainToTsQuery(searchTerm);
			var notes = await _context.Notes
	            .Where(p => p.SearchVector.Matches(EF.Functions.PlainToTsQuery(searchTerm)))
				.Select(n => new NoteUI()
				{
					Id = n.Id,
					Title = n.Title,
					Text = n.Text,
					CreationDate = n.CreationDate
				})
				.ToListAsync();
            if(!notes.Any()) 
            {
				// NpgsqlTsVector has no matches. Trying that method - slower but returns more results
				notes = await FindNotesWithEfCore(searchTerm);
            }
			return new NotesList()
			{
				Notes = notes,
				CurrentPage = 1,
				Pages = 0,
				DisplayType = DisplayType.SearchingByText
			};
		}
        private async Task<List<NoteUI>> FindNotesWithEfCore(string searchTerm)
        {
            return await _context.Notes
                .Where(n => n.Title.Contains(searchTerm) || n.Text.Contains(searchTerm))
				.Select(n => new NoteUI()
				{
					Id = n.Id,
					Title = n.Title,
					Text = n.Text,
					CreationDate = n.CreationDate
				})
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
        private int GetPageCount(float pageSize)
        {
            NoteCount = _context.Notes.Count();
			return (int)Math.Ceiling(NoteCount / pageSize);
        }
		public async Task<bool> UpdateNoteAsync(Note note)
		{
            try
            {
                _context.Update(note);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
		}
	}
}
