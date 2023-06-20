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
            ConfigureReadingListProperties();
            ConfigureListBookProperties();
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
            _modelBuilder.Entity<Book>().Property<DateTime>(d => d.PublicationDate).HasColumnType("Date").IsRequired();

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


        public void ConfigureReadingListProperties()
        {
            _modelBuilder.Entity<ReadingList>().Property<int>(d => d.Id).IsRequired();
            _modelBuilder.Entity<ReadingList>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<ReadingList>().Property<DateTime>(d => d.CreationDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<ReadingList>().Property<string>(d => d.Description).HasMaxLength(500);
            _modelBuilder.Entity<ReadingList>().Property<string>(d => d.Name).HasMaxLength(100).IsRequired();
        }

        public void ConfigureListBookProperties()
        {
            _modelBuilder.Entity<ReadingListBook>().HasKey(ec => ec.BookId);
            _modelBuilder.Entity<ReadingListBook>().HasKey(ec => ec.ReadingListId);
        }

        public void ConfigureFeedbackProperties()
        {
            _modelBuilder.Entity<Feedback>().Property<int>(d => d.Id).IsRequired();
            _modelBuilder.Entity<Feedback>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<Feedback>().Property<DateTime>(d => d.CreatedDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<Feedback>().Property<string>(d => d.Review).HasMaxLength(500);
            _modelBuilder.Entity<Feedback>().Property<Rating>(d => d.Rating).IsRequired();
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


            _modelBuilder.Entity<ReadingList>().HasMany(b => b.Books).WithMany(b => b.ReadingLists)
                .UsingEntity<ReadingListBook>(
                    ba => ba.HasOne(b => b.Book).WithMany().HasForeignKey(b => b.BookId),
                    ba => ba.HasOne(b => b.ReadingList).WithMany().HasForeignKey(ba => ba.ReadingListId),

                    ba => {
                        ba.ToTable("ReadingListBook");
                        ba.HasKey(b => new { b.ReadingListId, b.BookId });
                    }

                ) ;

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
                    Description = "Known for her works on the Harry Potter series"
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
                    Description = "Known for his poetic works, arguably one of the few authors that put Palestine on the map."
                },
                new Author()
                {
                    Id = 4,
                    Name = "Jane Austen",
                    Description = "Known for her classic novels like 'Pride and Prejudice' and 'Sense and Sensibility.'"
                },
                new Author()
                {
                    Id = 5,
                    Name = "George Orwell",
                    Description = "Known for his dystopian novel '1984' and the allegorical novella 'Animal Farm.'"
                },
                new Author()
                {
                    Id = 6,
                    Name = "Ernest Hemingway",
                    Description = "Known for his simple yet powerful writing style in works like 'The Old Man and the Sea' and 'A Farewell to Arms.'"
                },
                new Author()
                {
                    Id = 7,
                    Name = "Agatha Christie",
                    Description = "Known for her mystery novels featuring famous detectives like Hercule Poirot and Miss Marple."
                },
                new Author()
                {
                    Id = 8,
                    Name = "William Shakespeare",
                    Description = "Known for his plays like 'Romeo and Juliet,' 'Hamlet,' and 'Macbeth,' which are considered timeless classics."
                },
                new Author()
                {
                    Id = 9,
                    Name = "Virginia Woolf",
                    Description = "Known for her modernist novels like 'Mrs. Dalloway' and 'To the Lighthouse.'"
                },
                new Author()
                {
                    Id = 10,
                    Name = "J.R.R. Tolkien",
                    Description = "Known for his epic fantasy series 'The Lord of the Rings' and 'The Hobbit.'"
                }
            };
            _modelBuilder.Entity<Author>().HasData(authors);
        }

        private void SeedGenre()
        {
            List<Genre> genres = new List<Genre>()
            {
                new Genre()
                {
                    Id = 1,
                    Type = "History"
                },
                new Genre()
                {
                    Id = 2,
                    Type = "Poetry"
                },
                new Genre()
                {
                    Id = 3,
                    Type = "Fiction"
                },
                new Genre()
                {
                    Id = 4,
                    Type = "Fairy Tale"
                },
                new Genre()
                {
                    Id = 5,
                    Type = "Comedy"
                },
                new Genre()
                {
                    Id = 6,
                    Type = "Adventure"
                },
                new Genre()
                {
                    Id = 7,
                    Type = "Mystery"
                },
                new Genre()
                {
                    Id = 8,
                    Type = "Science Fiction"
                },
                new Genre()
                {
                    Id = 9,
                    Type = "Romance"
                },
                new Genre()
                {
                    Id = 10,
                    Type = "Thriller"
                },
                new Genre()
                {
                    Id = 11,
                    Type = "Biography"
                },
                new Genre()
                {
                    Id = 12,
                    Type = "Fantasy"
                },
                new Genre()
                {
                    Id = 13,
                    Type = "Historical Fiction"
                }
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
                    PublicationDate = DateTime.Parse("1999-01-01"),
                },
                new Book()
                {
                    Id = 2,
                    Title = "Harry Potter",
                    Description = "Young boy discovers he has mysterious powers, changes his whole life to explore its potential",
                    PublicationDate = DateTime.Parse("2001-02-02"),
                },
                new Book()
                {
                    Id = 3,
                    Title = "If I Were Another",
                    Description = "Compilation of Mahmoud Darwish's top-notch poetry",
                    PublicationDate = DateTime.Parse("1990-01-01"),
                },
                new Book()
                {
                    Id = 4,
                    Title = "Pride and Prejudice",
                    Description = "A classic romance novel set in 19th-century England.",
                    PublicationDate = DateTime.Parse("1813-01-28"),
                },
                new Book()
                {
                    Id = 5,
                    Title = "1984",
                    Description = "A dystopian novel depicting a totalitarian regime and the struggle for individual freedom.",
                    PublicationDate = DateTime.Parse("1949-06-08"),
                },
                new Book()
                {
                    Id = 6,
                    Title = "The Old Man and the Sea",
                    Description = "A novella about an old fisherman's battle with a giant marlin and his inner struggles.",
                    PublicationDate = DateTime.Parse("1952-09-01"),
                },
                new Book()
                {
                    Id = 7,
                    Title = "Murder on the Orient Express",
                    Description = "A mystery novel featuring the detective Hercule Poirot, who investigates a murder on a luxurious train.",
                    PublicationDate = DateTime.Parse("1934-01-01"),
                },
                new Book()
                {
                    Id = 8,
                    Title = "Hamlet",
                    Description = "A tragedy that follows the Prince of Denmark's quest for revenge after his father's murder.",
                    PublicationDate = DateTime.Parse("1603-01-01"),
                },
                new Book()
                {
                    Id = 9,
                    Title = "Mrs. Dalloway",
                    Description = "A modernist novel that explores the thoughts and experiences of various characters during a single day in London.",
                    PublicationDate = DateTime.Parse("1925-05-14"),
                },
                new Book()
                {
                    Id = 10,
                    Title = "The Lord of the Rings",
                    Description = "An epic fantasy trilogy set in the fictional world of Middle-earth, following a group of heroes on a quest to destroy a powerful ring.",
                    PublicationDate = DateTime.Parse("1954-07-29"),
                }
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
                new BookAuthor
                {
                    BookId = 4,
                    AuthorId = 4,
                },
                new BookAuthor
                {
                    BookId = 5,
                    AuthorId = 5,
                },
                new BookAuthor
                {
                    BookId = 6,
                    AuthorId = 6,
                },
                new BookAuthor
                {
                    BookId = 7,
                    AuthorId = 7,
                },
                new BookAuthor
                {
                    BookId = 8,
                    AuthorId = 8,
                },
                new BookAuthor
                {
                    BookId = 9,
                    AuthorId = 9,
                },
                new BookAuthor
                {
                    BookId = 10,
                    AuthorId = 10,
                }
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
                },
                new BookGenre
                {
                    BookId = 4,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 4,
                    GenreId = 11
                },
                new BookGenre
                {
                    BookId = 5,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 5,
                    GenreId = 8
                },
                new BookGenre
                {
                    BookId = 6,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 6,
                    GenreId = 11
                },
                new BookGenre
                {
                    BookId = 7,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 7,
                    GenreId = 7
                },
                new BookGenre
                {
                    BookId = 8,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 8,
                    GenreId = 9
                },
                new BookGenre
                {
                    BookId = 9,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 9,
                    GenreId = 12
                },
                new BookGenre
                {
                    BookId = 10,
                    GenreId = 3
                },
                new BookGenre
                {
                    BookId = 10,
                    GenreId = 12
                }
            };

            _modelBuilder.Entity<BookGenre>().HasData(bookGenres);
        }

    }
}