using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateReservationEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsApproved",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsApproved",
                value: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "rIgX/r25yVaRfQzajF0mU8ZhNGnrwDiEyEm3rrWFVWY=", "9BeErr+8GTt3jhp8z2v82A==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "7bVS6AIRZuTBLGMkBueFf8Gl1RsAvw0RZa9cjqZlmvI=", "oJgwevUMg9xpun80iW3MYA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "n4lu4a7Bbjf7Dt5W3+1R+e7utXUtM0P5/q64kFTwv24=", "PqZH/uMXmecTcwjEcYVkeg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HashedPassword", "Salt" },
                values: new object[] { "TJijSckn/jsxDar6mR57Q+umAW1bZUNHDQPMUgm5+8s=", "CdANP/YupvKF34Ajq4fnbw==" });
        }
    }
}
