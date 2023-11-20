using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheepRepository.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRedundantFollowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorAuthor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorAuthor",
                columns: table => new
                {
                    FolloweesAuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowersAuthorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorAuthor", x => new { x.FolloweesAuthorId, x.FollowersAuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FolloweesAuthorId",
                        column: x => x.FolloweesAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowersAuthorId",
                        column: x => x.FollowersAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorAuthor_FollowersAuthorId",
                table: "AuthorAuthor",
                column: "FollowersAuthorId");
        }
    }
}
