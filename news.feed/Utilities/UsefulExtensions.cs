namespace news.feed.Utilities;

public static class UsefulExtensions
{
    public static bool InRange<T>(this T value, T min, T max) where T : IComparable<T> =>
        value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
}