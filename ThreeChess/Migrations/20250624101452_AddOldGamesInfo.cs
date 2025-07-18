using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreeChess.Migrations
{
    /// <inheritdoc />
    public partial class AddOldGamesInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OldGameInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GameInfo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldGameInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOldGameInfos",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    OldGameInfoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOldGameInfos", x => new { x.AppUserId, x.OldGameInfoId });
                    table.ForeignKey(
                        name: "FK_UserOldGameInfos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOldGameInfos_OldGameInfos_OldGameInfoId",
                        column: x => x.OldGameInfoId,
                        principalTable: "OldGameInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOldGameInfos_OldGameInfoId",
                table: "UserOldGameInfos",
                column: "OldGameInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOldGameInfos");

            migrationBuilder.DropTable(
                name: "OldGameInfos");
        }
    }
}
