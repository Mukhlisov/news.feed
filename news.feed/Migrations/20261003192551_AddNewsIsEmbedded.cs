using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news.feed.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsIsEmbedded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_news_Program",
                table: "news");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmbedded",
                table: "news",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_news_Program_IsEmbedded_CreationTime",
                table: "news",
                columns: new[] { "Program", "IsEmbedded", "CreationTime" },
                descending: new[] { false, false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_news_Program_IsEmbedded_CreationTime",
                table: "news");

            migrationBuilder.DropColumn(
                name: "IsEmbedded",
                table: "news");

            migrationBuilder.CreateIndex(
                name: "IX_news_Program",
                table: "news",
                column: "Program");
        }
    }
}
