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
    [Migration("20230608114530_bookPublications")]
    partial class bookPublications
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
                            Description = "Known for her works on the harry potter series",
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
                            Description = "Known for his poetic works, debatabely one of the few authors that put Palestine on the map.",
                            Name = "Mahmoud Darwish"
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
                            Description = "Compilation of Mahmoud Darwish's top notch poetry",
                            IsAvailable = true,
                            PublicationDate = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "If I Were Another"
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
                            ReservationDate = new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            ReservationDate = new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            BookId = 3,
                            ReservationDate = new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3
                        },
                        new
                        {
                            Id = 4,
                            BookId = 2,
                            ReservationDate = new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1
                        },
                        new
                        {
                            Id = 5,
                            BookId = 3,
                            ReservationDate = new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2
                        },
                        new
                        {
                            Id = 6,
                            BookId = 1,
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
                            HashedPassword = "8+yVsHGxCUp0u+wpINwlvMwBuQEgaU6EHSPC/vpNHRg=",
                            Name = "Rayyan Asia",
                            PhoneNumber = "1234567890",
                            Role = 0,
                            Salt = "dwhppjJwa9EXlVTQJVDY+w=="
                        },
                        new
                        {
                            Id = 2,
                            Email = "raneen23fast@gmail.com",
                            HashedPassword = "5lgRkkqgT7kZrdLSk12nhGKVqnajdk1DWpEYOI7z3Zc=",
                            Name = "Raneen Asia",
                            PhoneNumber = "1234567890",
                            Role = 1,
                            Salt = "WrjCK7zpS78dFT7IuktsLw=="
                        },
                        new
                        {
                            Id = 3,
                            Email = "karam23fast@gmail.com",
                            HashedPassword = "1iAK0L89sFKQM0MA6B7koF2iAG0ogSELEFjcjezBgUA=",
                            Name = "karam Shawish",
                            PhoneNumber = "1234567890",
                            Role = 2,
                            Salt = "5fZSR7r2aPbg6uyqXrYcVA=="
                        },
                        new
                        {
                            Id = 4,
                            Email = "ramiRayyan@gmail.com",
                            HashedPassword = "vQ1cVvvVj819hmXmBGZqeJ+AUQ/TLhALiUYioN3KydA=",
                            Name = "Rami Asia",
                            PhoneNumber = "1234567890",
                            Role = 2,
                            Salt = "YLjnQ2midbHBAJQC+awR2g=="
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
