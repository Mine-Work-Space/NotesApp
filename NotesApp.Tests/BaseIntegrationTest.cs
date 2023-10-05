using Microsoft.Extensions.DependencyInjection;
using NotesApp.Data;
using NotesApp.Repositories.Interfaces;

namespace NotesApp.Tests
{
	/// <summary>
	///     Abstract class used as a base class for integration tests in the application. Implements the 
	///     <see cref="IClassFixture<IntegrationTestNoteDbFactory>"/> interface, 
	///     which suggests that it is designed to be used with xUnit.NET for setting up and tearing down resources shared among test classes
	/// </summary>
	public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestNoteDbFactory>
	{
		private readonly IServiceScope _scope;
        protected readonly INoteRepository NoteRepository;
        protected BaseIntegrationTest(IntegrationTestNoteDbFactory factory)
        {
            _scope = factory.Services.CreateScope();
            NoteRepository = _scope.ServiceProvider.GetRequiredService<INoteRepository>();
        }
    }
}
