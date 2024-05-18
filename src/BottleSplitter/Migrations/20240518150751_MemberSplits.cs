using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleSplitter.Migrations
{
    /// <inheritdoc />
    public partial class MemberSplits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SplitMembers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BottleSplitSplitterUser");

            migrationBuilder.CreateTable(
                name: "SplitMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SplitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    DateDeleted = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    DateUpdated = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SplitMembers_Splits_SplitId",
                        column: x => x.SplitId,
                        principalTable: "Splits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_SplitMembers_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_SplitMembers_MemberId",
                table: "SplitMembers",
                column: "MemberId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_SplitMembers_SplitId",
                table: "SplitMembers",
                column: "SplitId"
            );
        }
    }
}
