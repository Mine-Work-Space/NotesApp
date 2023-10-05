using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Data;
using Testcontainers.PostgreSql;

namespace NotesApp.Tests
{
	/// <summary>
	///		Used to set up and manage a PostgreSQL database container for integration testing purposes. 
	/// </summary>
	public class IntegrationTestNoteDbFactory : WebApplicationFactory<Program>, IAsyncLifetime
	{
		private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
			.WithImage("postgres")
			.WithDatabase("TestNoteDB")
			.WithPortBinding(5432)
			.WithUsername("postgres")
			.WithPassword("postgres")
			.Build();

		/// <summary>
		///		Configure test services: Remove the existing DbContextOptions and use DbContext
		///		with the test connection string.
		/// </summary>
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureTestServices(services =>
			{
				var descriptor = services
					.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DbManager>));
				// Remove DbManager with normal DbContextOptions
				if (descriptor is not null)
				{
					services.Remove(descriptor);
				}
				// Adding DbContext with "TestNoteDB" connection string
				services.AddDbContext<DbManager>(options =>
				{
					options.UseNpgsql(_dbContainer.GetConnectionString());
				});
			});
		}

		// Different method
		public new Task DisposeAsync()
		{
			return _dbContainer.StopAsync();
		}

		public Task InitializeAsync()
		{
			return _dbContainer.StartAsync();
		}
	} 
}
