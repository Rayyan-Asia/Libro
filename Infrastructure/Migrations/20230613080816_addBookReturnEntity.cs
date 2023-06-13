using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBookReturnEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookReturns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookReturns_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BookReturns_LoanId",
                table: "BookReturns",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReturns");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "qR0z6YKH/bgRjeExqyAvUV0evYWQht+yFQ4jvwo/f8k=", "Aa26H/xADKmX65baKNF/Eg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "i9QXuskXvfePOCgklKbvBSK3IIpXx0M9Zm1Ha6IFZuM=", "kZFZhwGbPCnK54ve0BJ8rQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "eAIGaIBD1qgXhffzS7M/ZM9AV9H8jhXB/nA44kSDfg0=", "I7bwPi9rxnC/iY81EDmgBg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "2b8uGXuDN92L66hnr4shJ3p2BDNBBj136bi+/2thQoM=", "AZ9u+KDSq9EHBV5Z7AlFpw==" });
        }
    }
}
