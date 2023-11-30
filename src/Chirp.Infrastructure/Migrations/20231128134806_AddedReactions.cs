using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheepRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddedReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheepId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReactionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ReactionContent = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => new { x.UserId, x.CheepId, x.ReactionType, x.ReactionContent });
                    table.ForeignKey(
                        name: "FK_Reactions_Cheeps_CheepId",
                        column: x => x.CheepId,
                        principalTable: "Cheeps",
                        principalColumn: "CheepId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CheepId",
                table: "Reactions",
                column: "CheepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");
        }
    }
}
