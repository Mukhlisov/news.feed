using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news.feed.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmetnsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "news",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "news_attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsBodyId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_news_attachment_news_body_NewsBodyId",
                        column: x => x.NewsBodyId,
                        principalTable: "news_body",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_news_attachment_NewsBodyId",
                table: "news_attachment",
                column: "NewsBodyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_attachment");

            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "news");
        }
    }
}
