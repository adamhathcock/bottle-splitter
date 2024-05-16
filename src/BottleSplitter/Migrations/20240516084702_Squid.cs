using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleSplitter.Migrations
{
    /// <inheritdoc />
    public partial class Squid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Squid",
                table: "Splits",
                type: "text",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Squid", table: "Splits");
        }
    }
}
