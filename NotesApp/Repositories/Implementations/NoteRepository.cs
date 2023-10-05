using AutoMapper;
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
		private readonly DbManager _context;
		public int NoteCount { get; private set; }
		private readonly IMapper _mapper;
		public NoteRepository(DbManager context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<NotesList> GetNotesByPageAsync(int page, float pageSize = 5f)
		{
			try
			{
				if (page < 1)
					throw new Exception();
				var pageCount = GetPageCount(pageSize);
				var notes = await _context.Notes
					.OrderBy(n => n.CreationDate)
					.Skip((page - 1) * (int)pageSize)
					.Take((int)pageSize)
					.Select(n => _mapper.Map<NoteUI>(n))
					.ToListAsync();
				return new NotesList()
				{
					Notes = notes,
					CurrentPage = page,
					Pages = pageCount,
					DisplayType = DisplayType.Default
				};
			}
			catch
			{
				return new NotesList() { };
			}
		}
		public async Task<NotesList> GetNotesBySearchTermAsync(string searchTerm)
		{
			try
			{
				var notes = await _context.Notes
					.Where(p => p.SearchVector.Matches(EF.Functions.PlainToTsQuery(searchTerm)))
					.Select(n => _mapper.Map<NoteUI>(n))
					.ToListAsync();
				if (!notes.Any() && searchTerm.Length > 5)
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
			catch
			{
				return new NotesList() { };
			}

		}
		private async Task<List<NoteUI>> FindNotesWithEfCore(string searchTerm)
		{
			return await _context.Notes
				.Where(n => n.Title.Contains(searchTerm) || n.Text.Contains(searchTerm))
				.Select(n => _mapper.Map<NoteUI>(n))
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
			catch (UniqueConstraintException ex)
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
		public async Task<bool> UpdateNoteAsync(Note updatedNote)
		{
			try
			{
				var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == updatedNote.Id);
				if (note == null)
					return false;
				else
				{
					note.Text = updatedNote.Text;
					await _context.SaveChangesAsync();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}
