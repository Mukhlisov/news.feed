using Microsoft.EntityFrameworkCore;
using news.feed.models.Models;

namespace news.feed.Config.EntityFramework;

public class NewsFeedContext : DbContext
{
    public NewsFeedContext(DbContextOptions<NewsFeedContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<models.Models.News> News { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<models.Models.News>(entity =>
        {
            entity.ToTable("News");
            entity.Property(news => news.AuthorId).IsRequired();
            entity.Property(news => news.Title).IsRequired();
            entity.Property(news => news.Body).IsRequired();
            entity.Property(news => news.Program).IsRequired();
        });
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.Property(attachment => attachment.ContentLink).IsRequired();
            entity.Property(attachment => attachment.ContentType).IsRequired();
            entity.HasOne(attachment => attachment.News).WithMany(news => news.Attachments);
        });
    }
}