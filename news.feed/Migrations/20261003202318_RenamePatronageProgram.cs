using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace news.feed.Migrations
{
    /// <inheritdoc />
    public partial class RenamePatronageProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Программы заполняются на старте только в пустую таблицу, поэтому существующие базы правим здесь
            migrationBuilder.Sql(
                "UPDATE news_program SET \"Name\" = 'Функциональная реабилитация' WHERE \"Alias\" = 'patronage';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE news_program SET \"Name\" = 'Проект «Патронаж»' WHERE \"Alias\" = 'patronage';");
        }
    }
}
