using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FolloweeId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_FolloweeId",
                table: "Followers");

            migrationBuilder.AddColumn<Guid>(
                name: "FolloweeUserId",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FollowerUserId",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FolloweeUserId",
                table: "Followers",
                column: "FolloweeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FollowerUserId",
                table: "Followers",
                column: "FollowerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FolloweeUserId",
                table: "Followers",
                column: "FolloweeUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowerUserId",
                table: "Followers",
                column: "FollowerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FolloweeUserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowerUserId",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_FolloweeUserId",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_FollowerUserId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "FolloweeUserId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "FollowerUserId",
                table: "Followers");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FolloweeId",
                table: "Followers",
                column: "FolloweeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FolloweeId",
                table: "Followers",
                column: "FolloweeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
