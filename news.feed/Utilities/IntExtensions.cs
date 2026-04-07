namespace news.feed.Utilities;

public static class IntExtensions
{
    public static bool InRange(this int value, int min, int max) =>
        value >= min && value <= max;
}