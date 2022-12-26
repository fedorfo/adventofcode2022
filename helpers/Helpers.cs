namespace adventofcode2022.helpers;

public static class Helpers
{
    public static int Max(params int[] values) => values.Max();

    public static long LongPow(long x, int y)
    {
        long result = 1;
        for (var i = 1; i <= y; i++)
            result *= x;
        return result;
    }

    public static int Mod(int a, int b)
    {
        var result = a % b;
        if (result < 0)
            result += b;
        return result;
    }

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