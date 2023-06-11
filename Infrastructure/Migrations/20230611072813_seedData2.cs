using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Known for her works on the Harry Potter series");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Known for his poetic works, arguably one of the few authors that put Palestine on the map.");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 4, "Known for her classic novels like 'Pride and Prejudice' and 'Sense and Sensibility.'", "Jane Austen" },
                    { 5, "Known for his dystopian novel '1984' and the allegorical novella 'Animal Farm.'", "George Orwell" },
                    { 6, "Known for his simple yet powerful writing style in works like 'The Old Man and the Sea' and 'A Farewell to Arms.'", "Ernest Hemingway" },
                    { 7, "Known for her mystery novels featuring famous detectives like Hercule Poirot and Miss Marple.", "Agatha Christie" },
                    { 8, "Known for his plays like 'Romeo and Juliet,' 'Hamlet,' and 'Macbeth,' which are considered timeless classics.", "William Shakespeare" },
                    { 9, "Known for her modernist novels like 'Mrs. Dalloway' and 'To the Lighthouse.'", "Virginia Woolf" },
                    { 10, "Known for his epic fantasy series 'The Lord of the Rings' and 'The Hobbit.'", "J.R.R. Tolkien" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Compilation of Mahmoud Darwish's top-notch poetry");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Description", "IsAvailable", "PublicationDate", "Title" },
                values: new object[,]
                {
                    { 4, "A classic romance novel set in 19th-century England.", true, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pride and Prejudice" },
                    { 5, "A dystopian novel depicting a totalitarian regime and the struggle for individual freedom.", true, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "1984" },
                    { 6, "A novella about an old fisherman's battle with a giant marlin and his inner struggles.", true, new DateTime(1952, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Old Man and the Sea" },
                    { 7, "A mystery novel featuring the detective Hercule Poirot, who investigates a murder on a luxurious train.", true, new DateTime(1934, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Murder on the Orient Express" },
                    { 8, "A tragedy that follows the Prince of Denmark's quest for revenge after his father's murder.", true, new DateTime(1603, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hamlet" },
                    { 9, "A modernist novel that explores the thoughts and experiences of various characters during a single day in London.", true, new DateTime(1925, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mrs. Dalloway" },
                    { 10, "An epic fantasy trilogy set in the fictional world of Middle-earth, following a group of heroes on a quest to destroy a powerful ring.", true, new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Lord of the Rings" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 7, "Mystery" },
                    { 8, "Science Fiction" },
                    { 9, "Romance" },
                    { 10, "Thriller" },
                    { 11, "Biography" },
                    { 12, "Fantasy" },
                    { 13, "Historical Fiction" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "Y8bt65erBnbGQbi20FGBCEkFtNAPvXxEiRNNIqzvePM=", "Goaf3rlz9PXHxnxcyplwaQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "WMu4tZ2uaO2BMi6UWGoRnzCO4mvezrCuGDZyPI6eN+E=", "xcFGCtR9ja08QckxQRJ3Ng==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "5pNhOKzcw/Zwb8/y/7KU3CdlzF24xG14q5/o61FG4us=", "pymQCLNGpXY7njPeYJBU4g==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "VKySEf0Rx//u7WdE5rkuv1tsMi+TNDfcuwswIree2CI=", "pBzsl8nx7vs0KED00Rcq8w==" });

            migrationBuilder.InsertData(
                table: "BookAuthor",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "BookGenre",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 4, 3 },
                    { 4, 11 },
                    { 5, 3 },
                    { 5, 8 },
                    { 6, 3 },
                    { 6, 11 },
                    { 7, 3 },
                    { 7, 7 },
                    { 8, 3 },
                    { 8, 9 },
                    { 9, 3 },
                    { 9, 12 },
                    { 10, 3 },
                    { 10, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 8, 8 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 9, 9 });

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 10, 10 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 4, 11 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 5, 8 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 6, 11 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 8, 9 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 9, 12 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 10, 12 });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Known for her works on the harry potter series");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Known for his poetic works, debatabely one of the few authors that put Palestine on the map.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Compilation of Mahmoud Darwish's top notch poetry");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "8+yVsHGxCUp0u+wpINwlvMwBuQEgaU6EHSPC/vpNHRg=", "dwhppjJwa9EXlVTQJVDY+w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "5lgRkkqgT7kZrdLSk12nhGKVqnajdk1DWpEYOI7z3Zc=", "WrjCK7zpS78dFT7IuktsLw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "1iAK0L89sFKQM0MA6B7koF2iAG0ogSELEFjcjezBgUA=", "5fZSR7r2aPbg6uyqXrYcVA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "vQ1cVvvVj819hmXmBGZqeJ+AUQ/TLhALiUYioN3KydA=", "YLjnQ2midbHBAJQC+awR2g==" });
        }
    }
}
