using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleSplitter.Migrations
{
    /// <inheritdoc />
    public partial class SplitMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BottleSplitSplitterUser");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SplitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: true),
                    Paid = table.Column<bool>(type: "boolean", nullable: true),
                    Shipped = table.Column<bool>(type: "boolean", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    DateUpdated = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    DateDeleted = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Splits_SplitId",
                        column: x => x.SplitId,
                        principalTable: "Splits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_SplitId",
                table: "Memberships",
                column: "SplitId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Memberships");

            migrationBuilder.DropColumn(name: "FirstName", table: "Users");

            migrationBuilder.DropColumn(name: "LastName", table: "Users");

            migrationBuilder.CreateTable(
                name: "BottleSplitSplitterUser",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "uuid", nullable: false),
                    SplitsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_BottleSplitSplitterUser",
                        x => new { x.MembersId, x.SplitsId }
                    );
                    table.ForeignKey(
                        name: "FK_BottleSplitSplitterUser_Splits_SplitsId",
                        column: x => x.SplitsId,
                        principalTable: "Splits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_BottleSplitSplitterUser_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_BottleSplitSplitterUser_SplitsId",
                table: "BottleSplitSplitterUser",
                column: "SplitsId"
            );
        }
    }
}
