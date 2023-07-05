using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class jobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackgroundJobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "7TccyIyvgoy59553m1h+BCUxb3aV2HYWf83uAJjOP5A=", "f3HQdmAeCTOGZpPSREp02g==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "/ehut5IUH8aB6mvQ8IjJhdpgZUfkIXRDLnzL3XavN/Q=", "6zeHQKXuaudRj0DOhV+BiA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "5PZpj0R4nlCfNpTHVwzgZ5wXHcSJM2MgF5dL+h5Ng7M=", "43T0uprNKtK3jJDxMNXlhg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "aD5XS5ZIVefbjgCbV+lRByig4KVhyPEw1OddeXGvqxY=", "m0f1/7IpZ0bLpF2LJUUxiA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_LoanId",
                table: "Jobs",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "LifsaElOrHyab5cWlqb1ro4yvS2evREsxTbEIERSsYE=", "+YAtRS6R9sw/1t47oyqJwA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "TX+WJ138jQCwmDEj0mKgQFVMyIS61tvT1shU5fZXXnA=", "bpBZNDTdyZTUErdJAMaOcg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "Qg5Ch0L39N7kYmDLaFmwgLHWeNR6KG+wMjuYVRt06sE=", "gdtuNi9PAtMf4k49tTd+xw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "l7lGLTO30/Vcx+pahZLAoYzk7Fp0dmiPiecAYmc5TZQ=", "JwFtTFLQ1d5mpI88w1RxHQ==" });
        }
    }
}
