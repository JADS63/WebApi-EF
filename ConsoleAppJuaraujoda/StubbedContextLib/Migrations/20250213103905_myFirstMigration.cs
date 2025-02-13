using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StubbedContextLib.Migrations
{
    /// <inheritdoc />
    public partial class myFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Height = table.Column<float>(type: "REAL", nullable: false),
                    Nationality = table.Column<string>(type: "TEXT", nullable: false),
                    Handplay = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "BirthDate", "FirstName", "Handplay", "Height", "LastName", "Nationality" },
                values: new object[,]
                {
                    { 42, new DateTime(1998, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aryna", 3, 1.82f, "Sabalenka", "Belarus" },
                    { 43, new DateTime(2001, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iga", 3, 1.76f, "Swiatek", "Poland" },
                    { 44, new DateTime(2004, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coco", 3, 1.75f, "Gauff", "USA" },
                    { 45, new DateTime(1996, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jasmine", 3, 1.63f, "Paolini", "Italy" },
                    { 46, new DateTime(2002, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Qinwen", 3, 1.78f, "Zheng", "China" },
                    { 47, new DateTime(1999, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elena", 3, 1.84f, "Rybakina", "Kazakhstan" },
                    { 48, new DateTime(1994, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jessica", 3, 1.7f, "Pegula", "USA" },
                    { 49, new DateTime(2001, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma", 3, 1.7f, "Navarro", "USA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
