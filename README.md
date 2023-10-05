# NotesApp

**The NotesApp is a web-based application designed to help users organize and manage their notes with ease. Built using Blazor Server, Entity Framework Core and PostgreSQL, this application offers a responsive user interface for creating, viewing, and editing notes.**

> Note: NotesApp is an *experimental* project. It's not a commercial product. 

To see project as template without any data (Database for PostgreSQL is expensive), check out [NotesApp demo view](https://blazornote.azurewebsites.net/)

## Getting Started

To get started with NoteApp and build project on your own machine, look at my recommendations below.

## Building the NotesApp

Prerequisites:
- [Node.js](https://nodejs.org/) (>8.3)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://docs.docker.com/desktop/install/mac-install/)

After completed prerequisites, follow these steps:
1. Set NotesApp as startup project
2. Set up connection string. Change in appsettings.json DefaultConnection with your parameters
```json 
  "ConnectionStrings": {
    "DefaultConnection": "USER ID=YOUR_ID; Password=YOUR_PASSWORD; Server=YOUR_SERVER; Port=YOUR_PORT; Database=DB_NAME; Integrated Security=true; Pooling=true;"
  }
```
2. Build the project
3. Open Package Manager Console and write ```update-database``` command. Notes table will be created in your db.
4. 👾 Start the project.

## Run unit tests

Project has simple unit tests. During the tests **another db** uses, so, to run unit tests, follow these steps:
1. Set NotesApp.Tests as startup project
2. Build the project.
3. Start Docker. NotesApp.Tests module uses PostgreSqlContainer
4. Change _dbContainer parametes to your own. NotesApp.Tests -> IntegrationTestNoteDbFactory. Field PostgreSqlContainer _dbContainer.
```csharp
		private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
			.WithImage("image_name")
			.WithDatabase("test_db_name")
			.WithPortBinding(port_integer)
			.WithUsername("username")
			.WithPassword("password")
			.Build();
```
6. Run tests

## Still got questions?

You can write me in Telegram. [sashaFPV](https://t.me/sasha_fpv).
