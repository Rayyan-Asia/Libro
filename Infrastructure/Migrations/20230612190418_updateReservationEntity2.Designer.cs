﻿// <auto-generated />
using System;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(LibroDbContext))]
    [Migration("20230612190418_updateReservationEntity2")]
    partial class updateReservationEntity2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Known for her works on the Harry Potter series",
                            Name = "J.K. Rowling"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Known for his Palestinian short stories and novels",
                            Name = "Ghasan Kanafani"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Known for his poetic works, arguably one of the few authors that put Palestine on the map.",
                            Name = "Mahmoud Darwish"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Known for her classic novels like 'Pride and Prejudice' and 'Sense and Sensibility.'",
                            Name = "Jane Austen"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Known for his dystopian novel '1984' and the allegorical novella 'Animal Farm.'",
                            Name = "George Orwell"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Known for his simple yet powerful writing style in works like 'The Old Man and the Sea' and 'A Farewell to Arms.'",
                            Name = "Ernest Hemingway"
                        },
                        new
                        {
                            Id = 7,
                            Description = "Known for her mystery novels featuring famous detectives like Hercule Poirot and Miss Marple.",
                            Name = "Agatha Christie"
                        },
                        new
                        {
                            Id = 8,
                            Description = "Known for his plays like 'Romeo and Juliet,' 'Hamlet,' and 'Macbeth,' which are considered timeless classics.",
                            Name = "William Shakespeare"
                        },
                        new
                        {
                            Id = 9,
                            Description = "Known for her modernist novels like 'Mrs. Dalloway' and 'To the Lighthouse.'",
                            Name = "Virginia Woolf"
                        },
                        new
                        {
                            Id = 10,
                            Description = "Known for his epic fantasy series 'The Lord of the Rings' and 'The Hobbit.'",
                            Name = "J.R.R. Tolkien"
                        });
                });

            modelBuilder.Entity("Domain.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("Date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Men seek refuge to find a better living.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Men In The Sun"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Young boy discovers he has mysterious powers, changes his whole life to explore its potential",
                            IsAvailable = true,
                            PublicationDate = new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Harry Potter"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Compilation of Mahmoud Darwish's top-notch poetry",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "If I Were Another"
                        },
                        new
                        {
                            Id = 4,
                            Description = "A classic romance novel set in 19th-century England.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Pride and Prejudice"
                        },
                        new
                        {
                            Id = 5,
                            Description = "A dystopian novel depicting a totalitarian regime and the struggle for individual freedom.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "1984"
                        },
                        new
                        {
                            Id = 6,
                            Description = "A novella about an old fisherman's battle with a giant marlin and his inner struggles.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1952, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Old Man and the Sea"
                        },
                        new
                        {
                            Id = 7,
                            Description = "A mystery novel featuring the detective Hercule Poirot, who investigates a murder on a luxurious train.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1934, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Murder on the Orient Express"
                        },
                        new
                        {
                            Id = 8,
                            Description = "A tragedy that follows the Prince of Denmark's quest for revenge after his father's murder.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1603, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Hamlet"
                        },
                        new
                        {
                            Id = 9,
                            Description = "A modernist novel that explores the thoughts and experiences of various characters during a single day in London.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1925, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Mrs. Dalloway"
                        },
                        new
                        {
                            Id = 10,
                            Description = "An epic fantasy trilogy set in the fictional world of Middle-earth, following a group of heroes on a quest to destroy a powerful ring.",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Lord of the Rings"
                        });
                });

            modelBuilder.Entity("Domain.BookAuthor", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("BookAuthor", (string)null);

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            AuthorId = 2
                        },
                        new
                        {
                            BookId = 2,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 3,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 4,
                            AuthorId = 4
                        },
                        new
                        {
                            BookId = 5,
                            AuthorId = 5
                        },
                        new
                        {
                            BookId = 6,
                            AuthorId = 6
                        },
                        new
                        {
                            BookId = 7,
                            AuthorId = 7
                        },
                        new
                        {
                            BookId = 8,
                            AuthorId = 8
                        },
                        new
                        {
                            BookId = 9,
                            AuthorId = 9
                        },
                        new
                        {
                            BookId = 10,
                            AuthorId = 10
                        });
                });

            modelBuilder.Entity("Domain.BookGenre", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("BookGenre", (string)null);

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 1,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 4
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 5
                        },
                        new
                        {
                            BookId = 2,
                            GenreId = 6
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 1
                        },
                        new
                        {
                            BookId = 3,
                            GenreId = 2
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 4,
                            GenreId = 11
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 5,
                            GenreId = 8
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 6,
                            GenreId = 11
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 7,
                            GenreId = 7
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 8,
                            GenreId = 9
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 9,
                            GenreId = 12
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 3
                        },
                        new
                        {
                            BookId = 10,
                            GenreId = 12
                        });
                });

            modelBuilder.Entity("Domain.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "History"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Poetry"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Fiction"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Fairy Tale"
                        },
                        new
                        {
                            Id = 5,
                            Type = "Comedy"
                        },
                        new
                        {
                            Id = 6,
                            Type = "Adventure"
                        },
                        new
                        {
                            Id = 7,
                            Type = "Mystery"
                        },
                        new
                        {
                            Id = 8,
                            Type = "Science Fiction"
                        },
                        new
                        {
                            Id = 9,
                            Type = "Romance"
                        },
                        new
                        {
                            Id = 10,
                            Type = "Thriller"
                        },
                        new
                        {
                            Id = 11,
                            Type = "Biography"
                        },
                        new
                        {
                            Id = 12,
                            Type = "Fantasy"
                        },
                        new
                        {
                            Id = 13,
                            Type = "Historical Fiction"
                        });
                });

            modelBuilder.Entity("Domain.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("Date");

                    b.Property<DateTime>("LoanDate")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("Date");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("isExcused")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Loans");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            DueDate = new DateTime(2023, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LoanDate = new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1,
                            isExcused = false
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            DueDate = new DateTime(2023, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LoanDate = new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2,
                            isExcused = false
                        },
                        new
                        {
                            Id = 3,
                            BookId = 3,
                            DueDate = new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LoanDate = new DateTime(2023, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3,
                            isExcused = false
                        });
                });

            modelBuilder.Entity("Domain.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPendingApproval")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("Date");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            BookId = 3,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3
                        },
                        new
                        {
                            Id = 4,
                            BookId = 2,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1
                        },
                        new
                        {
                            Id = 5,
                            BookId = 3,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2
                        },
                        new
                        {
                            Id = 6,
                            BookId = 1,
                            IsApproved = false,
                            IsPendingApproval = true,
                            ReservationDate = new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3
                        });
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "ray23fast@gmail.com",
                            HashedPassword = "qR0z6YKH/bgRjeExqyAvUV0evYWQht+yFQ4jvwo/f8k=",
                            Name = "Rayyan Asia",
                            PhoneNumber = "1234567890",
                            Role = 0,
                            Salt = "Aa26H/xADKmX65baKNF/Eg=="
                        },
                        new
                        {
                            Id = 2,
                            Email = "raneen23fast@gmail.com",
                            HashedPassword = "i9QXuskXvfePOCgklKbvBSK3IIpXx0M9Zm1Ha6IFZuM=",
                            Name = "Raneen Asia",
                            PhoneNumber = "1234567890",
                            Role = 1,
                            Salt = "kZFZhwGbPCnK54ve0BJ8rQ=="
                        },
                        new
                        {
                            Id = 3,
                            Email = "karam23fast@gmail.com",
                            HashedPassword = "eAIGaIBD1qgXhffzS7M/ZM9AV9H8jhXB/nA44kSDfg0=",
                            Name = "karam Shawish",
                            PhoneNumber = "1234567890",
                            Role = 2,
                            Salt = "I7bwPi9rxnC/iY81EDmgBg=="
                        },
                        new
                        {
                            Id = 4,
                            Email = "ramiRayyan@gmail.com",
                            HashedPassword = "2b8uGXuDN92L66hnr4shJ3p2BDNBBj136bi+/2thQoM=",
                            Name = "Rami Asia",
                            PhoneNumber = "1234567890",
                            Role = 2,
                            Salt = "AZ9u+KDSq9EHBV5Z7AlFpw=="
                        });
                });

            modelBuilder.Entity("Domain.BookAuthor", b =>
                {
                    b.HasOne("Domain.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Domain.BookGenre", b =>
                {
                    b.HasOne("Domain.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Domain.Loan", b =>
                {
                    b.HasOne("Domain.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Reservation", b =>
                {
                    b.HasOne("Domain.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
