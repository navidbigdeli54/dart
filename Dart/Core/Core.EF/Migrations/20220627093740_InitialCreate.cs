using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.EF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    EndPoint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblGameSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGameSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblGameSession_tblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblLeaderboard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLeaderboard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblLeaderboard_tblGameSession_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "tblGameSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GameSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Point = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblScore_tblGameSession_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "tblGameSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblGameSession_UserId",
                table: "tblGameSession",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblLeaderboard_GameSessionId",
                table: "tblLeaderboard",
                column: "GameSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblScore_GameSessionId",
                table: "tblScore",
                column: "GameSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblLeaderboard");

            migrationBuilder.DropTable(
                name: "tblScore");

            migrationBuilder.DropTable(
                name: "tblGameSession");

            migrationBuilder.DropTable(
                name: "tblUser");
        }
    }
}
