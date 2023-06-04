using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueEmailKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "FZHUUnTErWMMG63yRa4Uq5xNHagDOxrKj7BGIyfBRvs=", "2xOIUt8hfOGWl76c5s47Iw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "9B13MfD3KsByobMh4+K68I5pQDV4fA3/hsPulklppEg=", "hnnpoARPFhoE1FRkVshEuQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "SOOciZ2RxwuRIpZKY1MHOBfGIJFPC34pcEGn4//njk4=", "al7eThuHIdSLrx+BuFaLSA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "ZEGZhDfSRz6WePuNOJ+0H4OqZn0K5OFMx++b4ufdBd4=", "/o6HSkIy8V3f/PxjvWhIwA==" });
        }
    }
}
