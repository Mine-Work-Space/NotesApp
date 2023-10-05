using FluentAssertions;
using NotesApp.Models;

namespace NotesApp.Tests.Repositories.Implementations
{
	public class NoteRepositoryTests : BaseIntegrationTest
	{
		public NoteRepositoryTests(IntegrationTestNoteDbFactory factory)
			: base(factory) { }

		[Fact]
		public async void SaveNoteAsync_ShouldReturnTuple_TrueAndSuccessMessage()
		{
			// Arrange
			var note = new Note()
			{
				Id = Guid.NewGuid(),
				CreationDate = DateOnly.FromDateTime(DateTime.Now),
				Title = "Default title",
				Text = "Default text for default note"
			};
			// Act
			var result = await NoteRepository.SaveNoteAsync(note);
			// Assert
			var expectedResult = (true, "New note was successfully added!");
			result.Should().BeEquivalentTo(expectedResult);
		}
		[Theory]
		[InlineData("default")]
		public async void GetNotesBySearchTermAsync_ShouldReturnCustomList_WhenNoteFounded(string searchTerm)
		{
			// Act
			var result = await NoteRepository.GetNotesBySearchTermAsync(searchTerm);
			// Assert
			result.Should().NotBeNull();
			result.Notes.Should().HaveCount(c => c >= 1);
		}
		[Fact]
		public async void UpdateNoteAsync_ShouldReturnTrue()
		{
			// Arrange
			var notes = await NoteRepository.GetNotesByPageAsync(1, 1);
			var editableNote = notes.Notes!.FirstOrDefault();
			// Act
			editableNote!.Text = "Edited text";
			var result = await NoteRepository
				.UpdateNoteAsync(new Note()
			{
				Text = editableNote.Text,
				Id = editableNote.Id,
				CreationDate = editableNote.CreationDate,
				Title = editableNote.Title
			});
			// Assert
			result.Should().BeTrue();
		}
	}
}
