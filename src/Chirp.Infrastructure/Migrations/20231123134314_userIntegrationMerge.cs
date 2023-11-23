using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheepRepository.Migrations
{
    /// <inheritdoc />
    public partial class userIntegrationMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FolloweeAuthorAuthorId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FollowerAuthorAuthorId",
                table: "Followers");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.RenameColumn(
                name: "FollowerAuthorAuthorId",
                table: "Followers",
                newName: "FollowerUserId");

            migrationBuilder.RenameColumn(
                name: "FolloweeAuthorAuthorId",
                table: "Followers",
                newName: "FolloweeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerAuthorAuthorId",
                table: "Followers",
                newName: "IX_Followers_FollowerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FolloweeAuthorAuthorId",
                table: "Followers",
                newName: "IX_Followers_FolloweeUserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Cheeps",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_AuthorId",
                table: "Cheeps",
                newName: "IX_Cheeps_UserId");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Users_UserId",
                table: "Cheeps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Cheeps_Users_UserId",
                table: "Cheeps");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FolloweeUserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowerUserId",
                table: "Followers");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.RenameColumn(
                name: "FollowerUserId",
                table: "Followers",
                newName: "FollowerAuthorAuthorId");

            migrationBuilder.RenameColumn(
                name: "FolloweeUserId",
                table: "Followers",
                newName: "FolloweeAuthorAuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerUserId",
                table: "Followers",
                newName: "IX_Followers_FollowerAuthorAuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FolloweeUserId",
                table: "Followers",
                newName: "IX_Followers_FolloweeAuthorAuthorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Cheeps",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_UserId",
                table: "Cheeps",
                newName: "IX_Cheeps_AuthorId");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
