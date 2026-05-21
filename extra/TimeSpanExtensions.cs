namespace extra;

public static class TimeSpanExtensions
{
    public static TimeSpan Seconds(this int seconds) => TimeSpan.FromSeconds(seconds);
    public static TimeSpan Minutes(this int minutes) => TimeSpan.FromMinutes(minutes);
    public static  TimeSpan Days(this int days) => TimeSpan.FromDays(days);
}