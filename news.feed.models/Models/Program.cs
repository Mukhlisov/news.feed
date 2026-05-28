using System.ComponentModel.DataAnnotations;

namespace news.feed.models.Models;

public class Program
{
    [Key]
    public string Alias { get; set; }
    public string Name { get; set; }
    public virtual ICollection<News> News { get; set; } = new List<News>();
}