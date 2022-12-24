namespace adventofcode2022.helpers;

public static class Helpers
{
    public static int Max(params int[] values) => values.Max();

    public static void Measure(Action acc)
    {
        var now = DateTime.UtcNow;
        acc();
        Console.WriteLine(DateTime.UtcNow-now);
    }
    
    public static T Measure<T>(Func<T> func)
    {
        var now = DateTime.UtcNow;
        var result = func();
        Console.WriteLine(DateTime.UtcNow-now);
        return result;
    }
}