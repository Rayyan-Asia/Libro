using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Libro.Infrastructure
{
    public class LibroDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookeGenres { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }

        public LibroDbContext(DbContextOptions<LibroDbContext> options)
            : base(options)
        {
        }

        public LibroDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\localDB;Initial Catalog=Libro")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelManager modelManager = new ModelManager(modelBuilder);
            modelManager.ConfigureEntityProperties();
            modelManager.SeedData();
        }

    }
}