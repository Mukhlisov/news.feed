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
        modelBuilder.Entity<models.Models.News>().ToTable("News");
        modelBuilder.Entity<Attachment>().ToTable("Attachments");
    }
}