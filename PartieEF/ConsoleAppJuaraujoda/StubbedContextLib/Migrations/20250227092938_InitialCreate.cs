using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StubbedContextLib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Result = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEntityResultEntity",
                columns: table => new
                {
                    PlayerEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEntityResultEntity", x => new { x.PlayerEntityId, x.ResultEntityId });
                    table.ForeignKey(
                        name: "FK_PlayerEntityResultEntity_Players_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerEntityResultEntity_Results_ResultEntityId",
                        column: x => x.ResultEntityId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultEntityTournamentEntity",
                columns: table => new
                {
                    TournamentEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultEntityTournamentEntity", x => new { x.ResultEntityId, x.TournamentEntityId });
                    table.ForeignKey(
                        name: "FK_ResultEntityTournamentEntity_Results_ResultEntityId",
                        column: x => x.ResultEntityId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultEntityTournamentEntity_Tournaments_TournamentEntityId",
                        column: x => x.TournamentEntityId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "Id", "Result" },
                values: new object[,]
                {
                    { 42, 6 },
                    { 43, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "Name", "Year" },
                values: new object[,]
                {
                    { 42, "Rolland Garos", 2021 },
                    { 43, "Rolland Garos", 2022 }
                });

            migrationBuilder.InsertData(
                table: "PlayerEntityResultEntity",
                columns: new[] { "PlayerEntityId", "ResultEntityId" },
                values: new object[,]
                {
                    { 42, 42 },
                    { 43, 43 }
                });

            migrationBuilder.InsertData(
                table: "ResultEntityTournamentEntity",
                columns: new[] { "ResultEntityId", "TournamentEntityId" },
                values: new object[,]
                {
                    { 42, 42 },
                    { 43, 43 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntityResultEntity_ResultEntityId",
                table: "PlayerEntityResultEntity",
                column: "ResultEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultEntityTournamentEntity_TournamentEntityId",
                table: "ResultEntityTournamentEntity",
                column: "TournamentEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerEntityResultEntity");

            migrationBuilder.DropTable(
                name: "ResultEntityTournamentEntity");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
