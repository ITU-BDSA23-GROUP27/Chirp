using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheepRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddedFollowers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follower_Authors_FolloweeAuthorAuthorId",
                table: "Follower");

            migrationBuilder.DropForeignKey(
                name: "FK_Follower_Authors_FollowerAuthorAuthorId",
                table: "Follower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follower",
                table: "Follower");

            migrationBuilder.RenameTable(
                name: "Follower",
                newName: "Followers");

            migrationBuilder.RenameIndex(
                name: "IX_Follower_FollowerAuthorAuthorId",
                table: "Followers",
                newName: "IX_Followers_FollowerAuthorAuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Follower_FolloweeAuthorAuthorId",
                table: "Followers",
                newName: "IX_Followers_FolloweeAuthorAuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                columns: new[] { "FollowerId", "FolloweeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Authors_FolloweeAuthorAuthorId",
                table: "Followers",
                column: "FolloweeAuthorAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Authors_FollowerAuthorAuthorId",
                table: "Followers",
                column: "FollowerAuthorAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FolloweeAuthorAuthorId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FollowerAuthorAuthorId",
                table: "Followers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.RenameTable(
                name: "Followers",
                newName: "Follower");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerAuthorAuthorId",
                table: "Follower",
                newName: "IX_Follower_FollowerAuthorAuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FolloweeAuthorAuthorId",
                table: "Follower",
                newName: "IX_Follower_FolloweeAuthorAuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follower",
                table: "Follower",
                columns: new[] { "FollowerId", "FolloweeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_Authors_FolloweeAuthorAuthorId",
                table: "Follower",
                column: "FolloweeAuthorAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_Authors_FollowerAuthorAuthorId",
                table: "Follower",
                column: "FollowerAuthorAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
