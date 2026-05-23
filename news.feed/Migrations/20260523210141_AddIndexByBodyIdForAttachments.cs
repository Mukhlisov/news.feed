using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news.feed.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexByBodyIdForAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_news_attachment_NewsBodyId",
                table: "news_attachment",
                newName: "IX_attachments_NewsBodyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_attachments_NewsBodyId",
                table: "news_attachment",
                newName: "IX_news_attachment_NewsBodyId");
        }
    }
}
