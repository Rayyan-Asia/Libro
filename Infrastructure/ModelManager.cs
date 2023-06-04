using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Libro.Infrastructure
{
    public class ModelManager
    {
        private readonly ModelBuilder _modelBuilder;

        public ModelManager(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }


        public void ConfigureEntityProperties()
        {
            ConfigureUserProperties();
            ConfigureAuthorProperties();
            ConfigureGenreProperties();
            ConfigureBookProperties();
            ConfigureReservationProperties();
            ConfigureLoanProperties();
            ConfigureBookAuthorProperties();
            ConfigureBookGenreProperties();
            ConfigureManytoManyRelationships();
        }


        public void SeedData()
        {
            SeedUser();
            SeedAuthor();
            SeedGenre();
            SeedBook();
            SeedReservation();
            SeedLoan();
            SeedBookAuthor();
            SeedBookGenre();
        }

        private void ConfigureUserProperties()
        {
            _modelBuilder.Entity<User>().Property<Role>(d => d.Role).IsRequired();
            _modelBuilder.Entity<User>().Property<string>(d => d.Name).HasMaxLength(32).IsRequired();
            _modelBuilder.Entity<User>().Property(d => d.PhoneNumber).HasMaxLength(16).IsRequired();
            _modelBuilder.Entity<User>().Property(d => d.Email).HasMaxLength(100).IsRequired();
            _modelBuilder.Entity<User>().HasIndex(d => d.Email).IsUnique();
        }
        private void ConfigureAuthorProperties()
        {
            _modelBuilder.Entity<Author>().Property<string>(d => d.Description).HasMaxLength(500);
            _modelBuilder.Entity<Author>().Property<string>(d => d.Name).HasMaxLength(32).IsRequired();
        }

        private void ConfigureGenreProperties()
        {
            _modelBuilder.Entity<Genre>().Property<string>(d => d.Type).HasMaxLength(32);
        }
        private void ConfigureBookProperties()
        {
            _modelBuilder.Entity<Book>().Property<string>(d => d.Description).HasMaxLength(500);
            _modelBuilder.Entity<Book>().Property<string>(d => d.Title).HasMaxLength(100).IsRequired();
            _modelBuilder.Entity<Book>().Property<bool>(d => d.IsAvailable).HasDefaultValue(false);
        }

        private void ConfigureReservationProperties()
        {
            _modelBuilder.Entity<Reservation>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<Reservation>().Property<int>(d => d.BookId).IsRequired();
            _modelBuilder.Entity<Reservation>().Property<DateTime>(d => d.ReservationDate).HasColumnType("Date").IsRequired();
        }

        private void ConfigureLoanProperties()
        {
            _modelBuilder.Entity<Loan>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<Loan>().Property<int>(d => d.BookId).IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime>(d => d.LoanDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime>(d => d.DueDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime?>(d => d.ReturnDate).HasColumnType("Date");
        }

        private void ConfigureBookAuthorProperties()
        {
            _modelBuilder.Entity<BookAuthor>().HasKey(ec => ec.BookId);
            _modelBuilder.Entity<BookAuthor>().HasKey(ec => ec.AuthorId);
        }

        private void ConfigureBookGenreProperties()
        {
            _modelBuilder.Entity<BookGenre>().HasKey(ec => ec.BookId);
            _modelBuilder.Entity<BookGenre>().HasKey(ec => ec.GenreId);
        }
        public void ConfigureManytoManyRelationships()
        {

            _modelBuilder.Entity<Book>()
                        .HasMany(b => b.Authors)
                        .WithMany(b => b.Books)
                        .UsingEntity<BookAuthor>
                        (
                            ba => ba.HasOne(b => b.Author)
                            .WithMany()
                            .HasForeignKey(b => b.AuthorId),

                            ba => ba.HasOne(b => b.Book)
                                 .WithMany()
                                 .HasForeignKey(b => b.BookId),
                            ba =>
                            {
                                ba.ToTable("BookAuthor");
                                ba.HasKey(b => new { b.BookId, b.AuthorId });
                            }
                        );

            _modelBuilder.Entity<Book>()
                        .HasMany(b => b.Genres)
                        .WithMany(b => b.Books)
                        .UsingEntity<BookGenre>
                        (
                            ba => ba.HasOne(b => b.Genre)
                            .WithMany()
                            .HasForeignKey(b => b.GenreId),

                            ba => ba.HasOne(b => b.Book)
                                 .WithMany()
                                 .HasForeignKey(b => b.BookId),
                            ba =>
                            {
                                ba.ToTable("BookGenre");
                                ba.HasKey(b => new { b.BookId, b.GenreId });
                            }
                        );

        }

        private void SeedUser()
        {
            List<string> passwords = new List<string>()
            {
                "Rayyan123",
                "Raneen123",
                "Karam123",
                "Rami123",
            };
            List<User> users = new List<User>(){
                new User()
                {Id = 1,
                 Name = "Rayyan Asia",
                 PhoneNumber = "1234567890",
                 Email = "ray23fast@gmail.com",
                 Role = Role.Administrator,
                },
                new User()
                {Id = 2,
                 Name = "Raneen Asia",
                 PhoneNumber = "1234567890",
                 Email = "raneen23fast@gmail.com",
                 Role = Role.Librarian,
                },
                new User()
                {Id = 3,
                 Name = "karam Shawish",
                 PhoneNumber = "1234567890",
                 Email = "karam23fast@gmail.com",
                 Role = Role.Patron,
                },
                new User()
                {Id = 4,
                 Name = "Rami Asia",
                 PhoneNumber = "1234567890",
                 Email = "ramiRayyan@gmail.com",
                 Role = Role.Patron,
                },
            };
            HashPasswords(users, passwords);
            _modelBuilder.Entity<User>().HasData(users);
        }
        private void HashPasswords(List<User> users, List<string> passwords)
        {
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                string salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.ComputeHash(passwords[i], salt);
                user.HashedPassword = hashedPassword;
                user.Salt = salt;
            }
        }

        private void SeedAuthor()
        {
            List<Author> authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    Name = "J.K. Rowling",
                    Description = "Known for her works on the harry potter series"
                },
                new Author()
                {
                    Id = 2,
                    Name = "Ghasan Kanafani",
                    Description = "Known for his Palestinian short stories and novels"
                },
                new Author()
                {
                    Id = 3,
                    Name = "Mahmoud Darwish",
                    Description = "Known for his poetic works, debatabely one of the few authors that put Palestine on the map."
                },
            };

            _modelBuilder.Entity<Author>().HasData(authors);
        }

        private void SeedGenre()
        {
            List<Genre> genres = new List<Genre>()
            {
                new Genre()
                {
                    Id= 1,
                    Type = "History"
                },
                new Genre()
                {
                    Id= 2,
                    Type = "Poetry"
                },
                new Genre()
                {
                    Id= 3,
                    Type = "Fiction"
                },
                new Genre()
                {
                    Id= 4,
                    Type = "Fairy Tale"
                },
                new Genre()
                {
                    Id= 5,
                    Type = "Comedy"
                },
                new Genre()
                {
                    Id= 6,
                    Type = "Adventure"
                },
            };

            _modelBuilder.Entity<Genre>().HasData(genres);
        }

        private void SeedBook()
        {
            List<Book> books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Men In The Sun",
                    Description = "Men seek refuge to find a better living.",
                },
                new Book()
                {
                    Id = 2,
                    Title = "Harry Potter",
                    Description = "Young boy discovers he has mysterious powers, changes his whole life to explore its potential",
                },
                new Book()
                {
                    Id = 3,
                    Title = "If I Were Another",
                    Description = "Compilation of Mahmoud Darwish's top notch poetry",
                },
            };

            _modelBuilder.Entity<Book>().HasData(books);
        }

        private void SeedReservation()
        {
            List<Reservation> reservations = new List<Reservation>()
            {
                new Reservation() {
                    Id = 1,
                    UserId = 1,
                    BookId = 1,
                    ReservationDate = DateTime.Parse("2023-4-10"),
                },
                new Reservation() {
                    Id = 2,
                    UserId = 2,
                    BookId = 2,
                    ReservationDate = DateTime.Parse("2023-4-11"),
                },
                new Reservation() {
                    Id = 3,
                    UserId = 3,
                    BookId = 3,
                    ReservationDate = DateTime.Parse("2023-4-11"),
                },
                new Reservation() {
                    Id = 4,
                    UserId = 1,
                    BookId = 2,
                    ReservationDate = DateTime.Parse("2023-5-10"),
                },
                new Reservation() {
                    Id = 5,
                    UserId = 2,
                    BookId = 3,
                    ReservationDate = DateTime.Parse("2023-5-11"),
                },
                new Reservation() {
                    Id = 6,
                    UserId = 3,
                    BookId = 1,
                    ReservationDate = DateTime.Parse("2023-5-11"),
                }
            };

            _modelBuilder.Entity<Reservation>().HasData(reservations);
        }

        private void SeedLoan()
        {
            List<Loan> loans = new List<Loan>()
            {
                new Loan()
                {
                    Id = 1,
                    BookId = 1,
                    UserId = 1,
                    LoanDate = DateTime.Parse("2023-4-10"),
                    DueDate = DateTime.Parse("2023-4-24"),
                },
                new Loan()
                {
                    Id = 2,
                    BookId = 2,
                    UserId = 2,
                    LoanDate = DateTime.Parse("2023-4-11"),
                    DueDate = DateTime.Parse("2023-4-25"),
                },
                new Loan()
                {
                    Id = 3,
                    BookId = 3,
                    UserId = 3,
                    LoanDate = DateTime.Parse("2023-4-12"),
                    DueDate = DateTime.Parse("2023-4-26"),
                },
            };

            _modelBuilder.Entity<Loan>().HasData(loans);
        }

        private void SeedBookAuthor()
        {
            List<BookAuthor> bookAuthors = new List<BookAuthor>()
             {
                 new BookAuthor
                 {
                     BookId = 1,
                     AuthorId = 2,
                 },
                 new BookAuthor
                 {
                     BookId = 2,
                     AuthorId = 1,
                 },
                 new BookAuthor
                 {
                     BookId = 3,
                     AuthorId = 3,
                 },
             };
            _modelBuilder.Entity<BookAuthor>().HasData(bookAuthors);
        }
        private void SeedBookGenre()
        {
            List<BookGenre> bookGenres = new List<BookGenre>()
            {
                new BookGenre
                {
                    BookId = 1,
                    GenreId = 1
                },
                new BookGenre
                {
                    BookId = 1,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 2,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 2,
                    GenreId = 4
                },
                new BookGenre
                {
                    BookId = 2,
                    GenreId = 5
                },
                new BookGenre
                {
                    BookId = 2,
                    GenreId = 6
                },
                new BookGenre
                {
                    BookId = 3,
                    GenreId = 1
                },
                new BookGenre
                {
                    BookId = 3,
                    GenreId = 2
                }
            };

            _modelBuilder.Entity<BookGenre>().HasData(bookGenres);
        }

    }
}