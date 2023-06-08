using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class bookPublications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsAvailable", "PublicationDate" },
                values: new object[] { true, new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsAvailable", "PublicationDate" },
                values: new object[] { true, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsAvailable", "PublicationDate" },
                values: new object[] { true, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "gNxaWcY13aAwd0GCbHc7S8WePG55T8iMt3cXzWTKMzQ=", "3PZN96IFyO3s9rqcZdJ4ow==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "2+siIdPSiXy8YdFnNA1saGWmN3trJE4chezSP5zbsC8=", "/PBVITRjOpHfO34khiAGiw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "IcfP865u/yQ9ZubbwDNvLi4bi//3ZmCGQSbrlp2RK8w=", "CZFrrxhOB4j0mP3QI3doLg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "CmHkNtS31StGrMerPoiu73CRcHQrmMNOO69wmGEdS/U=", "wikucp7BkGOMPh8wdJXm5A==" });
        }
    }
}
