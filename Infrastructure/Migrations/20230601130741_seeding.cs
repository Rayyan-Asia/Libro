using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDate",
                table: "Reservations",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Loans",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LoanDate",
                table: "Loans",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Loans",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "isExcused",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Genres",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsReserved",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Known for her works on the harry potter series", "J.K. Rowling" },
                    { 2, "Known for his Palestinian short stories and novels", "Ghasan Kanafani" },
                    { 3, "Known for his poetic works, debatabely one of the few authors that put Palestine on the map.", "Mahmoud Darwish" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "BookId", "GenreId", "Type" },
                values: new object[,]
                {
                    { 1, null, null, "History" },
                    { 2, null, null, "Poetry" },
                    { 3, null, null, "Fiction" },
                    { 4, null, null, "Fairy Tale" },
                    { 5, null, null, "Comedy" },
                    { 6, null, null, "Adventure" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "HashedPassword", "Name", "PhoneNumber", "Role", "Salt" },
                values: new object[,]
                {
                    { 1, "ray23fast@gmail.com", "evfQSjHsAr36gyfN5jbIhI7Jmo1Fc+h3x9bywMHCVPA=", "Rayyan Asia", "1234567890", 0, "DNbYaNionAOQGbn7C8TpQA==" },
                    { 2, "raneen23fast@gmail.com", "Cj70poLlzgaLXuDZbY8D6MY0wpPT6n6+alj624uIQqg=", "Raneen Asia", "1234567890", 1, "8s9X4Ne2qwjLcWUL2kzBig==" },
                    { 3, "karam23fast@gmail.com", "2FfSlseQMwRz8kWoMuCatOm5KlLd8d2Nip1ZTJnGTsk=", "karam Shawish", "1234567890", 2, "etqGYj+hjUB+FvrYPEQ7Lg==" },
                    { 4, "ramiRayyan@gmail.com", "/rUhNhSNoWp07gf7cgo8lGGA8/1AfHmSIUmmmg/DB90=", "Rami Asia", "1234567890", 2, "+E0csp8nvS2nYwtxnc/XVA==" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Description", "Title" },
                values: new object[,]
                {
                    { 1, 2, "Men seek refuge to find a better living.", "Men In The Sun" },
                    { 2, 1, "Young boy discovers he has mysterious powers, changes his whole life to explore its potential", "Harry Potter" },
                    { 3, 3, "Compilation of Mahmoud Darwish's top notch poetry", "If I Were Another" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookId", "DueDate", "LoanDate", "ReturnDate", "UserId", "isExcused" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, false },
                    { 2, 2, new DateTime(2023, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, false },
                    { 3, 3, new DateTime(2023, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, false }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "BookId", "ReservationDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 3, new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 2, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, 3, new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, 1, new DateTime(2023, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_GenreId",
                table: "Genres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Genres_GenreId",
                table: "Genres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Genres_GenreId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_GenreId",
                table: "Genres");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "isExcused",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Genres");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Loans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LoanDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<bool>(
                name: "IsReserved",
                table: "Books",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
