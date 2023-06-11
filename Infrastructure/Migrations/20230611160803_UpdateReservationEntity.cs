using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPendingApproval",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPendingApproval",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPendingApproval",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPendingApproval",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsPendingApproval",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsPendingApproval",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsPendingApproval",
                value: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPendingApproval",
                table: "Reservations");

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
        }
    }
}
