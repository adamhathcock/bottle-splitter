using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleSplitter.Migrations
{
    /// <inheritdoc />
    public partial class Sizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Description", table: "Splits");

            migrationBuilder.DropColumn(name: "DetailsUrl", table: "Splits");

            migrationBuilder.DropColumn(name: "ImageUrl", table: "Splits");

            migrationBuilder.DropColumn(name: "TotalAvailable", table: "Splits");

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Splits",
                type: "jsonb",
                nullable: false,
                defaultValue: ""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Settings", table: "Splits");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Splits",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "DetailsUrl",
                table: "Splits",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Splits",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "TotalAvailable",
                table: "Splits",
                type: "integer",
                nullable: true
            );
        }
    }
}
