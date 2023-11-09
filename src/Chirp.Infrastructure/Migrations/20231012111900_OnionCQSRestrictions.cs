using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheepRepository.Migrations
{
    /// <inheritdoc />
    public partial class OnionCQSRestrictions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follower",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "INTEGER", nullable: false),
                    FolloweeId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowerAuthorAuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    FolloweeAuthorAuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follower", x => new { x.FollowerId, x.FolloweeId });
                    table.ForeignKey(
                        name: "FK_Follower_Authors_FolloweeAuthorAuthorId",
                        column: x => x.FolloweeAuthorAuthorId,
                        principalTable: "Authors",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follower_Authors_FollowerAuthorAuthorId",
                        column: x => x.FollowerAuthorAuthorId,
                        principalTable: "Authors",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follower_FolloweeAuthorAuthorId",
                table: "Follower",
                column: "FolloweeAuthorAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_FollowerAuthorAuthorId",
                table: "Follower",
                column: "FollowerAuthorAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follower");
        }
    }
}
