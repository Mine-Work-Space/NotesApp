using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NotesApp.Models.DbModels;

namespace NotesApp.Data
{
    public class DbManager : DbContext
    {
        public DbManager(DbContextOptions<DbManager> options) : base(options) {}
        public DbSet<Note> Notes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // For getting concrete Exception message, not DbException
            // See details https://github.com/Giorgi/EntityFramework.Exceptions
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
