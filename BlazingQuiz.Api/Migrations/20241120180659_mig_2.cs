using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingQuiz.Api.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("25377bd0-fb32-4ce3-a888-8764d8c80cd3"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsApproved", "Name", "PasswordHash", "Phone", "Role" },
                values: new object[] { new Guid("a25c99a8-389d-4eb4-9e88-ce414812b6da"), "admin@gmail.com", true, "Mehmet Aybey", "AQAAAAIAAYagAAAAEPAY3+pOYkqqk9HaQTXxkrcDo3Yp1m713gPsXgpTbPMbsns0lQzz/D7qKd5+PclA+Q==", "1234567890", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a25c99a8-389d-4eb4-9e88-ce414812b6da"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsApproved", "Name", "PasswordHash", "Phone", "Role" },
                values: new object[] { new Guid("25377bd0-fb32-4ce3-a888-8764d8c80cd3"), "admin@gmail.com", true, "Mehmet Aybey", "AQAAAAIAAYagAAAAEK1EAVoMiSqPCrChhRzWbKnExGfKrCZo64YSXFn059SVuiz4VAjW2/mmIC2IWEaNMg==", "1234567890", "Admin" });
        }
    }
}
