using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news.feed.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_attachments_NewsBodyId",
                table: "news_attachment");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "news_attachment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_attachments_NewsBodyId",
                table: "news_attachment",
                columns: new[] { "NewsBodyId", "Position" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_attachments_NewsBodyId",
                table: "news_attachment");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "news_attachment");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_NewsBodyId",
                table: "news_attachment",
                column: "NewsBodyId");
        }
    }
}
