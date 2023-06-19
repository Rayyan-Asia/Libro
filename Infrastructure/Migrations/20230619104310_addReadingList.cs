using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReadingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingListBook",
                columns: table => new
                {
                    ReadingListId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingListBook", x => new { x.ReadingListId, x.BookId });
                    table.ForeignKey(
                        name: "FK_ReadingListBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingListBook_ReadingLists_ReadingListId",
                        column: x => x.ReadingListId,
                        principalTable: "ReadingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "8LNTmiyn/g0pAh2VhxPEr2a8y939Lotr7jUH0yyGa/w=", "9grPXs877YruVxPSvKVINQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "xHGdf1pWjgPFz0Mrih5rhOrCSOSrJ7xyD3WwDJKtiMs=", "994hZCzf74e9mBQIS+pOlg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "4T+uD3iAG5jQl82MbMZ+8wT4lcLQwJB5SmBhVBuALpE=", "Rb4HkIAurBRZVi3ueEmwGA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "fhphu8VDOjo1n1v+qroCgem/RoFClHD5R5Ksmy16dDM=", "lFZQ20x/wq8CY63LyCoTyg==" });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingListBook_BookId",
                table: "ReadingListBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLists_UserId",
                table: "ReadingLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingListBook");

            migrationBuilder.DropTable(
                name: "ReadingLists");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "O0MsyfrhzXNFl0QQhkTNx/wROvhGc8pJIpJCLS/7RPM=", "BIlUSnegQ6zJpFFlptJb/Q==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "58LQxRfncHrg1uGtysYDj1WEXVCY3/MtoiQMuRXjkbw=", "trMtGP3V6jfF3vyHNNrZBg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "NlgjOobI9Xo1E6dvuh22FBKjHt4yjtmvcq2lrsxgpAo=", "MRCwQjRNuL18wdRK1HPcNQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "MR5RK7iWmW6EQtjEsIW0XFmLcUJ6OBykG5zNe8l4s1w=", "tD0gG1DJm6gcke5CJTeGxA==" });
        }
    }
}
