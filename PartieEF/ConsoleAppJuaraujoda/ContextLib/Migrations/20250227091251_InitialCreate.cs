using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContextLib.Migrations
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
