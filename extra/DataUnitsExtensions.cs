namespace extra;
/// <summary>
/// Возвращает размер данных выраженный в байтах
/// </summary>
public static class DataUnitsExtensions
{
    public static long Megabytes(this int value) => value.Kilobytes() * 1024;
    public static long Kilobytes(this int value) => value * 1024;
}