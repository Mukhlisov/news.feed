using Microsoft.EntityFrameworkCore;
using news.feed.Config.Settings;
using news.feed.models.Models;

namespace news.feed.Config.EntityFramework;

public class NewsFeedContext : DbContext
{
    public NewsFeedContext(DbContextOptions<NewsFeedContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<News> News { get; set; }
    public DbSet<NewsBody> NewsBodies { get; set; }
    public DbSet<models.Models.Program> Programs { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("news");
            entity.HasKey(news => news.Id);
            entity.Property(news => news.AuthorId)
                .IsRequired();
            entity.Property(news => news.Title)
                .HasMaxLength(150)
                .IsRequired();
            entity.Property(news => news.BodyId) // Не стал делать FK, чтобы не подтягивать за собой тело новости
                .IsRequired();
            entity.Property(news => news.Program)
                .HasMaxLength(AppSettings.DataBase.ProgramAliasLength)
                .IsRequired();
            entity.Property(news => news.CreationTime)
                .IsRequired();
            entity.Property(news => news.UpdateTime)
                .IsRequired();
            entity.HasIndex(news => news.Program, "IX_news_Program");
        });
        modelBuilder.Entity<NewsBody>(entity =>
        {
            entity.ToTable("news_body");
            entity.HasKey(newsBody => newsBody.Id);
            entity.Property(newsBody => newsBody.Body)
                .IsRequired();
            entity.HasMany(newsBody => newsBody.Attachments)
                .WithOne()
                .HasForeignKey(attachment => attachment.NewsBodyId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<models.Models.Program>(entity =>
        {
            entity.ToTable("news_program");
            entity.HasKey(program => program.Alias);
            entity.Property(program => program.Alias)
                .HasMaxLength(AppSettings.DataBase.ProgramAliasLength)
                .IsRequired();
            entity.HasIndex(program => program.Alias)
                .IsUnique();
            entity.Property(program => program.Name)
                .HasMaxLength(120)
                .IsRequired();
            entity.HasMany(program => program.News)
                .WithOne()
                .HasForeignKey(news => news.Program)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("news_attachment");
            entity.HasKey(attachment => attachment.Id);
        });
    }
}